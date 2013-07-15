;

(function(window, $) {
  var $fn = $.fn;

  $fn.overlay = function() {
    return this.each(function() {
      $(this).each(function() {
        var $this = $(this);

        $this.on('mouseover', function() {
          var $overlay = $this.find('.overlay');
          if ($overlay.length == 0) {
            //$this.append('<div class=overlay><span>Voir toute la collection ></span></div>');
            $this.append('<div class=overlay><img src="assets/css/img/' + $this.attr('rel') + '.png" /><span>Voir toute la collection ></span></div>');
            $overlay = $this.find('.overlay');
          }
          $overlay.show();
        }).on('mouseout', function() {
          $this.find('.overlay').hide();
        });
      });
    });
  };

  var app = {
    main: function() {
      // On document ready.
      $(function() {
        $('a[rel=external]').click(function() {
          window.open(this.href);
          return false;
        });
      });

      // Home
      $('#home #mosaic A').overlay();

      // Contact
      $('#contact .member A').overlay();
    }
  }
  ;

  window.App = app;

})(this, jQuery);
