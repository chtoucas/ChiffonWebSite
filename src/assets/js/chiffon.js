;

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
        '<div class=overlay></div><div class=watermark><span>'
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
    $('.mosaic').removeClass('shadow');
    $('.vignette').watermark(watermark);
  };

  /* Routes
   * ======================================================================= */

  chiffon.routes = {};

  chiffon.routes.home = function() {
    chiffon.ui.mosaic('%home.watermark');
  };

  chiffon.routes.member = function() {
    chiffon.ui.mosaic('%member.watermark');
  };

  return chiffon;

})(this, jQuery, chiffon);
