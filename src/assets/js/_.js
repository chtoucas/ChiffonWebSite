/*!
 * jQuery Cookie Plugin v1.3.1
 * https://github.com/carhartl/jquery-cookie
 *
 * Copyright 2013 Klaus Hartl
 * Released under the MIT license
 */
(function (factory) {
	if (typeof define === 'function' && define.amd) {
		// AMD. Register as anonymous module.
		define(['jquery'], factory);
	} else {
		// Browser globals.
		factory(jQuery);
	}
}(function ($) {

	var pluses = /\+/g;

	function raw(s) {
		return s;
	}

	function decoded(s) {
		return decodeURIComponent(s.replace(pluses, ' '));
	}

	function converted(s) {
		if (s.indexOf('"') === 0) {
			// This is a quoted cookie as according to RFC2068, unescape
			s = s.slice(1, -1).replace(/\\"/g, '"').replace(/\\\\/g, '\\');
		}
		try {
			return config.json ? JSON.parse(s) : s;
		} catch(er) {}
	}

	var config = $.cookie = function (key, value, options) {

		// write
		if (value !== undefined) {
			options = $.extend({}, config.defaults, options);

			if (typeof options.expires === 'number') {
				var days = options.expires, t = options.expires = new Date();
				t.setDate(t.getDate() + days);
			}

			value = config.json ? JSON.stringify(value) : String(value);

			return (document.cookie = [
				config.raw ? key : encodeURIComponent(key),
				'=',
				config.raw ? value : encodeURIComponent(value),
				options.expires ? '; expires=' + options.expires.toUTCString() : '', // use expires attribute, max-age is not supported by IE
				options.path    ? '; path=' + options.path : '',
				options.domain  ? '; domain=' + options.domain : '',
				options.secure  ? '; secure' : ''
			].join(''));
		}

		// read
		var decode = config.raw ? raw : decoded;
		var cookies = document.cookie.split('; ');
		var result = key ? undefined : {};
		for (var i = 0, l = cookies.length; i < l; i++) {
			var parts = cookies[i].split('=');
			var name = decode(parts.shift());
			var cookie = decode(parts.join('='));

			if (key && key === name) {
				result = converted(cookie);
				break;
			}

			if (!key) {
				result[name] = converted(cookie);
			}
		}

		return result;
	};

	config.defaults = {};

	$.removeCookie = function (key, options) {
		if ($.cookie(key) !== undefined) {
			// Must not alter options, thus extending a fresh object...
			$.cookie(key, '', $.extend({}, options, { expires: -1 }));
			return true;
		}
		return false;
	};

}));
/*
 * l10n.js
 * 2013-04-18
 * 
 * By Eli Grey, http://eligrey.com
 * Licensed under the X11/MIT License
 *   See LICENSE.md
 */

/*global XMLHttpRequest, setTimeout, document, navigator, ActiveXObject*/

/*! @source http://purl.eligrey.com/github/l10n.js/blob/master/l10n.js*/

(function () {
"use strict";

var
  undef_type = "undefined"
, string_type = "string"
, nav = self.navigator
, String_ctr = String
, has_own_prop = Object.prototype.hasOwnProperty
, load_queues = {}
, localizations = {}
, FALSE = !1
, TRUE = !0
// the official format is application/vnd.oftn.l10n+json, though l10n.js will also
// accept application/x-l10n+json and application/l10n+json
, l10n_js_media_type = /^\s*application\/(?:vnd\.oftn\.|x-)?l10n\+json\s*(?:$|;)/i
, XHR

// property minification aids
, $locale = "locale"
, $default_locale = "defaultLocale"
, $to_locale_string = "toLocaleString"
, $to_lowercase = "toLowerCase"

, array_index_of = Array.prototype.indexOf || function (item) {
	var
	  len = this.length
	, i   = 0
	;
	
	for (; i < len; i++) {
		if (i in this && this[i] === item) {
			return i;
		}
	}
	
	return -1;
}
, request_JSON = function (uri) {
	var req = new XHR();
	
	// sadly, this has to be blocking to allow for a graceful degrading API
	req.open("GET", uri, FALSE);
	req.send(null);
	
	if (req.status !== 200) {
		// warn about error without stopping execution
		setTimeout(function () {
			// Error messages are not localized as not to cause an infinite loop
			var l10n_err = new Error("Unable to load localization data: " + uri);
			l10n_err.name = "Localization Error";
			throw l10n_err;
		}, 0);
		
		return {};
	} else {
		return JSON.parse(req.responseText);
	}
}
, load = String_ctr[$to_locale_string] = function (data) {
	// don't handle function[$to_locale_string](indentationAmount:Number)
	if (arguments.length > 0 && typeof data !== "number") {
		if (typeof data === string_type) {
			load(request_JSON(data));
		} else if (data === FALSE) {
			// reset all localizations
			localizations = {};
		} else {
			// Extend current localizations instead of completely overwriting them
			var locale, localization, message;
			for (locale in data) {
				if (has_own_prop.call(data, locale)) {
					localization = data[locale];
					locale = locale[$to_lowercase]();
					
					if (!(locale in localizations) || localization === FALSE) {
						// reset locale if not existing or reset flag is specified
						localizations[locale] = {};
					}
					
					if (localization === FALSE) {
						continue;
					}
					
					// URL specified
					if (typeof localization === string_type) {
						if (String_ctr[$locale][$to_lowercase]().indexOf(locale) === 0) {
							localization = request_JSON(localization);
						} else {
							// queue loading locale if not needed
							if (!(locale in load_queues)) {
								load_queues[locale] = [];
							}
							load_queues[locale].push(localization);
							continue;
						}
					}
					
					for (message in localization) {
						if (has_own_prop.call(localization, message)) {
							localizations[locale][message] = localization[message];
						}
					}
				}
			}
		}
	}
	// Return what function[$to_locale_string]() normally returns
	return Function.prototype[$to_locale_string].apply(String_ctr, arguments);
}
, process_load_queue = function (locale) {
	var
	  queue = load_queues[locale]
	, i = 0
	, len = queue.length
	, localization
	;
	
	for (; i < len; i++) {
		localization = {};
		localization[locale] = request_JSON(queue[i]);
		load(localization);
	}
	
	delete load_queues[locale];
}
, use_default
, localize = String_ctr.prototype[$to_locale_string] = function () {
	var
	  using_default = use_default
	, current_locale = String_ctr[using_default ? $default_locale : $locale]
	, parts = current_locale[$to_lowercase]().split("-")
	, i = parts.length
	, this_val = this.valueOf()
	, locale
	;

	use_default = FALSE;
	
	// Iterate through locales starting at most-specific until a localization is found
	do {
		locale = parts.slice(0, i).join("-");
		// load locale if not loaded
		if (locale in load_queues) {
			process_load_queue(locale);
		}
		if (locale in localizations && this_val in localizations[locale]) {
			return localizations[locale][this_val];
		}
	}
	while (i --> 1);
	
	if (!using_default && String_ctr[$default_locale]) {
		use_default = TRUE;
		return localize.call(this_val);
	}

	return this_val;
}
;

if (typeof XMLHttpRequest === undef_type && typeof ActiveXObject !== undef_type) {
	var AXO = ActiveXObject;
	
	XHR = function () {
		try {
			return new AXO("Msxml2.XMLHTTP.6.0");
		} catch (xhrEx1) {}
		try {
			return new AXO("Msxml2.XMLHTTP.3.0");
		} catch (xhrEx2) {}
		try {
			return new AXO("Msxml2.XMLHTTP");
		} catch (xhrEx3) {}
	
		throw new Error("XMLHttpRequest not supported by this browser.");
	};
} else {
	XHR = XMLHttpRequest;
}

String_ctr[$default_locale] = String_ctr[$default_locale] || "";
String_ctr[$locale] = nav && (nav.language || nav.userLanguage) || "";

if (typeof document !== undef_type) {
	var
	  elts = document.getElementsByTagName("link")
	, i = elts.length
	, localization
	;
	
	while (i--) {
		var
		  elt = elts[i]
		, rel = (elt.getAttribute("rel") || "")[$to_lowercase]().split(/\s+/)
		;
		
		if (l10n_js_media_type.test(elt.type)) {
			if (array_index_of.call(rel, "localizations") !== -1) {
				// multiple localizations
				load(elt.getAttribute("href"));
			} else if (array_index_of.call(rel, "localization") !== -1) {
				// single localization
				localization = {};
				localization[(elt.getAttribute("hreflang") || "")[$to_lowercase]()] =
					elt.getAttribute("href");
				load(localization);
			}
		}
	}
}

}());
;

String.toLocaleString({
  'fr': {
    '%ajax.loading': 'En cours'
    , '%ajax.done': 'Terminé'
    , '%ajax.temp_error': 'Un problème temporaire est intervenue. Veuillez réessayer plus tard'
    , '%ajax.fatal_error': 'Une erreur est intervenue. Veuillez réessayer plus tard'
    , '%home.watermark': 'Voir toute la collection &gt;'
  }
  , 'en': {
    '%ajax.loading': 'Loading'
    , '%ajax.done': 'Done'
    , '%ajax.temp_error': 'A temporary error occured. Please try again later'
    , '%ajax.fatal_error': 'An error occured. Please try again later'
    , '%home.watermark': 'Go to the collection &gt;'
  }
});;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
//    https://github.com/kriskowal/es5-shim/

(function(window, $, chiffon, undef) {
  'use strict';

  var
    connected = undef !== $.cookie('auth')

    , visitor = {
      connected: connected
      , anonymous: !connected

      , logOn: function() {
        throw 'Not Implemented';
      }

      , logOff: function() {
        this.connected = false;
        this.anonymous = true;
      }
    }

     // L10N
    , _ = function(string) { return string.toLocaleString(); }
  ;

  /* jQuery plugins
   * ======================================================================= */

  $.fn.watermark = function(watermark) {
    return this.each(function() {
      $(this).append(
        //'<div class=overlay></div><div class=watermark><span>'
        '<div class=watermark><span>'
        + _(watermark)
        + '</span>');
    });
  };

  /* Chiffon object
   * ======================================================================= */

  chiffon.config = function(options) {
    options = $.extend({}, chiffon.config.defaults, options);

    // Pick up the locale from the HTML declaration and if not found use the default locale.
    chiffon.locale($('html').attr('lang') || options.defaultLocale);

    // Configure Ajax.
    chiffon.ajaxSetup(options.ajaxTimeout);

    return chiffon;
  };

  chiffon.config.defaults = {
    ajaxTimeout: 3000
    , defaultLocale: 'fr'
  }

  // Configure L10N.
  chiffon.locale = function(locale) {
    String.locale = locale;
  };

  // Configure jQuery ajax.
  chiffon.ajaxSetup = function(timeout) {
    $.ajaxSetup({
      timeout: timeout
      , async: true
      , cache: true
    });
  };

  chiffon.handle = function(route, params) {
    chiffon.ui.init();

    if (chiffon.routes.hasOwnProperty(route)) {
      chiffon.routes[route](params);
    }
  };

  /* UI
   * ======================================================================= */

  chiffon.ui = {};

  chiffon.ui.init = function() {
    // Open external links in a new window.
    $('A[rel=external]').click(function() {
      window.open(this.href);
      return false;
    });

    chiffon.ui.ajaxStatus();
    chiffon.ui.overlay.init();

    if (visitor.anonymous) {
      makeModal('register');

      //$.get('modal/register.html', function(data) { $modal.html(data); });
      //$modal.appendTo('BODY');

      // FIXME: contient modal pas =.
      $('A[rel~=modal]').click(function(e) {
        e.preventDefault();

        chiffon.ui.modal.register.show();

        // TODO: Use the Deferred jqXHR?
        $.ajax({
          type: 'GET'
          , global: false
          , dataType: 'html'
          , url: this.href
          , success: function(data) {
            var response = $('<html />').html(data);
            $('.register').html(response.find('#content').html());
            chiffon.ui.overlay.show();
          }
        });
      });
    }
  };

  function makeModal(name) {
    var $modal = $('<div class="modal register"></div>');
    $modal.appendTo('BODY');

    chiffon.ui.modal[name] = {
      show: function() {
        $modal.show();
        $modal.css('margin-top', -$modal.height() / 2);
        $modal.css('margin-left', -$modal.width() / 2);
      }
    };
  };

  chiffon.ui.modal = {};

  chiffon.ui.overlay = (function() {
    var $overlay = $('<div class=overlay></div>')

    return {
      init: function() {
        $overlay.appendTo('BODY');
        $overlay.height(screen.height);
        $overlay.width(screen.width);
      }

      , show: function() {
        $overlay.fadeIn();
      }

      , hide: function() {
        $overlay.hide();
      }
    };
  })();

  // Create & configure the ajax status placeholder.
  chiffon.ui.ajaxStatus = function() {
    var $status = $('<div id=ajax_status></div>')
      , error = false;

    $status.appendTo('BODY');

    $(document).ajaxStart(function() {
      $status
        .removeClass('error')
        .text(_('%ajax.loading'))
        .show();
    }).ajaxStop(function() {
      if (error) {
        error = false;
      } else {
        //$status.text(_('%ajax.done')).fadeOut('slow');
        $status.fadeOut('slow');
      }
    }).ajaxError(function(e, req) {
      var message = _(0 == req.status ? '%ajax.temp_error' : '%ajax.fatal_error');

      error = true;
      $status
        .text(message)
        .addClass('error')
        .show()
        .fadeOut(5000);
    });
  };

  chiffon.ui.mosaic = function(watermark) {
    //$('.mosaic').removeClass('shadow');
    $('.vignette').watermark(watermark);
  };

  /* Routes
   * ======================================================================= */

  chiffon.routes = {};

  chiffon.routes.home_index = function() {
    chiffon.ui.mosaic('%home.watermark');
  };

  return chiffon;

})(this, jQuery, chiffon);
