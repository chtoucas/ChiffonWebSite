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
    , self = { UI: {} }

    , UI = self.UI

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

  self.main = function() {
    $(function() {
      self.init();

      // Page key.
      var key = $('body').attr('id');

      if (UI.hasOwnProperty(key)) {
        UI[key]();
      }
    });
  };

  self.init = function() {
    // Initialize the correct locale.
    String.locale = $('html').attr('lang') || _defaultLocale;

    // Open external links in a new window.
    $('a[rel=external]').click(function() {
      window.open(this.href);
      return false;
    });
  };

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
