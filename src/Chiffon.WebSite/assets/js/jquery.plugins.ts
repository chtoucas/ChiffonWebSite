/*global window: true, jQuery:true*/

interface WatermarkOptions {
  wrapperStart?: string;
  wrapperEnd?: string;
}

interface JQuery {
  external(): any;
  watermark(watermark?: string, options?: WatermarkOptions): any;
}

((win: Window, $: JQueryStatic) => {
  'use strict';

  $.fn.external = function() {
    return this.each(function() {
      $(this).click(function() {
        win.open(this.href);
        return false;
      });
    });
  };

  $.fn.watermark = function(watermark?: string, options?: WatermarkOptions) {
    var settings: WatermarkOptions = $.extend({}, $.fn.watermark.defaults, options)
      , getWatermak = null !== watermark
      ? () => { return watermark; }
      // Si aucun texte n'est fourni, on utilise la valeur de l'attribut 'data-watermark'.
      : ($elmt: JQuery) => { return $elmt.data('watermark'); };

    return this.each(function() {
      var $this = $(this);
      $this.append(settings.wrapperStart + getWatermak($this) + settings.wrapperEnd);
    });
  };

  $.fn.watermark.defaults = {
    wrapperStart: '<div class=watermark><span>'
    , wrapperEnd: '</span></div>'
  };
})(this, jQuery);
