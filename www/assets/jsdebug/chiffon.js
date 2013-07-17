;

// TODO:
// - ajouter html5shiv.js
//
(function(root, $) {
  "use strict";

  var
    // Dependencies
    $fn = $.fn

    // Global objects
    , Window = root

    // Private properties
    , _defaultLocale = "fr"

    // Private methods
    // L10N
    , _ = function(string) { return string.toLocaleString(); }
  ;

  $fn.watermark = function(watermark) {
    return this.each(function() {
      $(this).append('<div class=overlay></div><div class=watermark><span>' + watermark + '</span>');
    });
  };

  var Chiffon = {};

  // Layout
  Chiffon.main = function() {
    $(function() {
      String.locale = $("html").attr("lang") || _defaultLocale;

      // Open external links in a new window.
      $('a[rel=external]').click(function() {
        Window.open(this.href);
        return false;
      });
    });
  };

  // Home page
  Chiffon.home = function() {
    Chiffon.main();

    $('.vignette').watermark(_("%home.watermark"));
  };

  Window.Chiffon = Chiffon;

})(this, jQuery);
