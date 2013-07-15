;

(function(window, $) {
  var $fn = $.fn;

  $fn.overlay = function() {
    return this.each(function() {
      $(this).each(function() {
        var $this = $(this);

        $this.on('mouseover', function() {
          var $hover = $this.find('.hover');
          if ($hover.length == 0) {
            $this.append('<div class=hover><div class=overlay></div><div class=info><span>Voir toute la collection ></span></div></div>');
              // <img src="assets/img/ui/' + $this.attr('rel') + '.png" />
            //$this.append('<div class=overlay><img src="assets/css/img/' + $this.attr('rel') + '.png" /><span>Voir toute la collection ></span></div>');
            $hover = $this.find('.hover');
          }
          $hover.show();
        }).on('mouseout', function() {
          $this.find('.hover').hide();
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
      //$('#contact .member A').overlay();
    }
  }
  ;

  window.App = app;

})(this, jQuery);
