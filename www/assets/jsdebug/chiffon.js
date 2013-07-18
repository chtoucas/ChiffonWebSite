;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
//    https://github.com/kriskowal/es5-shim/

var Chiffon = (function(window, $) {
  'use strict';

  var
    // Dependencies
    $fn = $.fn

    // Chiffon object
    , self = { UI: {}, User: {} }

    , UI = self.UI

    , User = self.User

    , _defaultLocale = 'fr'

    // L10N
    , _ = function(string) { return string.toLocaleString(); }
  ;

  /* jQuery plugins.
   * ======================================================================= */

  $fn.watermark = function(watermark) {
    return this.each(function() {
      $(this).append(
        '<div class=overlay></div><div class=watermark><span>'
        + _(watermark)
        + '</span>');
    });
  };


  /* Chiffon
   * ======================================================================= */

  self.main = function(route, opts) {
    $(function() {
      self.init();

      if (UI.hasOwnProperty(route)) {
        UI[route](opts);
      }
    });
  };

  self.init = function() {
    // Initialize the correct locale.
    String.locale = $('html').attr('lang') || _defaultLocale;

    // Open external links in a new window.
    $('A[rel=external]').click(function() {
      window.open(this.href);
      return false;
    });

    // Global overlay.
    var $overlay = $('<div class=overlay></div>')
    $overlay.appendTo('body');

    if (User.anonymous) {
      //var $modal = $('<div class="modal register"></div>');

      //$.get('modal/register.html', function(data) { $modal.html(data); });

      //$modal.appendTo('body');

      $('A[rel=modal]').click(function(e) {
        e.preventDefault();

        var $this = $(this);
        var $modal = $('<div class="modal register"></div>');
        $.get($this.attr('href'), function(data) { window.___ = $(data).find('h1'); });
        $modal.appendTo('body')
        return;

        $modal.show();
        $modal.css('margin-top', -$modal.height() / 2);
        $modal.css('margin-left', -$modal.width() / 2);
        $overlay.show();
      });
    }
  };

  /* Chiffon.User
   * ======================================================================= */

  // FIXME
  User.anonymous = true;

  /* Chiffon.UI
   * ======================================================================= */

  UI.home = function() {
    $('.vignette').watermark('%home.watermark');
    $('.mosaic').removeClass('shadow');
  };

  UI.member = function() {
    $('.vignette').watermark('%member.watermark');
    $('.mosaic').removeClass('shadow');

  };

  return self;

})(this, jQuery);
