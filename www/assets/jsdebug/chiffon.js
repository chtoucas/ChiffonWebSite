;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
//    https://github.com/kriskowal/es5-shim/

var Chiffon = (function(window, $) {
  'use strict';

  var
    // Namespace

    self = {}

    // L10N
    , _ = function(string) { return string.toLocaleString(); }
  ;

  /* jQuery.fn
   * ======================================================================= */

  $.fn.watermark = function(watermark) {
    return this.each(function() {
      $(this).append(
        '<div class=overlay></div><div class=watermark><span>'
        + _(watermark)
        + '</span>');
    });
  };

  /* Chiffon
   * ======================================================================= */

  self.settings = {
    defaultLocale: 'fr'
    , ajaxTimeout: 3000
  };

  self.main = function(route, opts) {
    //opts = $.extend({}, self.settings, opts);

    self.init();

    if (self.routes.hasOwnProperty(route)) {
      self.routes[route](opts);
    }
  };

  self.init = function() {
    // Initialize the correct locale.
    String.locale = $('html').attr('lang') || self.settings.defaultLocale;

    // Open external links in a new window.
    $('A[rel=external]').click(function() {
      window.open(this.href);
      return false;
    });

    // Configure Ajax.
    self.ajaxSetup();
    self.ajaxStatus();

    // Global overlay.
    var $overlay = $('<div class=overlay></div>')
    $overlay.appendTo('BODY');

    if (self.user.anonymous) {
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

  /* Ajax
   * ======================================================================= */

  self.ajaxSetup = function() {
    $.ajaxSetup({
      timeout: self.settings.ajaxTimeout
      , async: true
      , cache: true
    });
  };

  // Create & configure the ajax status placeholder.
  self.ajaxStatus = function() {
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

  /* User
   * ======================================================================= */

  // FIXME
  self.user = { anonymous: true };

  /* Routes
   * ======================================================================= */

  self.routes = {};

  self.routes.home = function() {
    $('.vignette').watermark('%home.watermark');
    $('.mosaic').removeClass('shadow');
  };

  self.routes.member = function() {
    $('.vignette').watermark('%member.watermark');
    $('.mosaic').removeClass('shadow');
  };

  return self;

})(this, jQuery);
