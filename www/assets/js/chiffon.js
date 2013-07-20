;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
//    https://github.com/kriskowal/es5-shim/

(function(window, $, chiffon, undef) {
  'use strict';

  var
    //
    _connected = undef !== $.cookie('auth')

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

  chiffon.visitor = {
    connected: _connected
    , anonymous: !_connected

    , logOn: function() {
      throw 'Not Implemented';
    }

    , logOff: function() {
      this.connected = false;
      this.anonymous = true;
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

    // Global overlay.
    var $overlay = $('<div class=overlay></div>')
    $overlay.appendTo('BODY');

    if (chiffon.visitor.anonymous) {
      //var $modal = $('<div class="modal register"></div>');
      //$.get('modal/register.html', function(data) { $modal.html(data); });
      //$modal.appendTo('BODY');

      $('A[rel=modal]').click(function(e) {
        e.preventDefault();

        //var $this = $(this);

        // TODO: Use the Deferred jqXHR?
        $.ajax({
          type: 'GET'
          , global: false
          , dataType: 'html'
          , url: this.href
          , success: function(data) {
            console.log($('#content', data).length);
          }
        });

        //$modal.show();
        //$modal.css('margin-top', -$modal.height() / 2);
        //$modal.css('margin-left', -$modal.width() / 2);
        //$overlay.show();
      });
    }
  };

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
