/*global window, jQuery*/

(function(window, $) {
  'use strict';

  $.fn.external = function() {
    return this.each(function() {
      $(this).click(function() {
        window.open(this.href);
        return false;
      });
    });
  };

  $.fn.watermark = function(watermark, options) {
    var settings = $.extend({}, $.fn.watermark.defaults, options);
    var getWatermak = null !== watermark ? function() {
      return watermark;
    } : function($elmt) {
      return $elmt.data('watermark');
    };

    return this.each(function() {
      var $this = $(this);
      $this.append(settings.wrapperStart + getWatermak($this) + settings.wrapperEnd);
    });
  };

  $.fn.watermark.defaults = {
    wrapperStart: '<div class=watermark><span>',
    wrapperEnd: '</span></div>'
  };
})(window, jQuery);
