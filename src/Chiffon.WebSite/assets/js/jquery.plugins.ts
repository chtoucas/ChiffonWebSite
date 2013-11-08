/*global jQuery*/

// TODO: null vs undefined.

/// <reference path="../../Scripts/typings/jquery/jquery.d.ts"/>

interface WatermarkOptions {
  wrapperStart?: string;
  wrapperEnd?: string;
}

interface JQuery {
  external(): any;
  watermark(watermark?: string, options?: WatermarkOptions): any;
}

(($: JQueryStatic) => {
  'use strict';

  $.fn.external = function() {
    return this.each(function() {
      $(this).click(function() {
        window.open(this.href);
        return false;
      });
    });
  };

  $.fn.watermark = function(watermark?: string, options?: WatermarkOptions) {
    var settings: WatermarkOptions = $.extend({}, $.fn.watermark.defaults, options)
      , get_watermak = null !== watermark
        ? () => { return watermark; }
        // Si aucun texte n'est fourni, on utilise la valeur de l'attribut 'data-watermark'.
        : ($elmt: JQuery) => { return $elmt.data('watermark'); };

    return this.each(function() {
      var $this = $(this);
      $this.append(settings.wrapperStart + get_watermak($this) + settings.wrapperEnd);
    });
  };

  $.fn.watermark.defaults = {
    wrapperStart: '<div class=watermark><span>'
    , wrapperEnd: '</span></div>'
  };
})(jQuery);
