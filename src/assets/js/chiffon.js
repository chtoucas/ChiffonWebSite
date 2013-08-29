;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
//    https://github.com/kriskowal/es5-shim/
// http://stackoverflow.com/questions/12779565/comparing-popular-script-loaders-yepnope-requirejs-labjs-and-headjs
// http://benalman.com/code/projects/jquery-resize/docs/files/jquery-ba-resize-js.html
// http://stackoverflow.com/questions/4298612/jquery-how-to-call-resize-event-only-once-its-finished-resizing

(function(window, $, chiffon, undef) {
  'use strict';

  var
     // L10N
    _ = function(string) { return string.toLocaleString(); }

    , _designer_sticky_info = function() {
      var
        // Géométrie du bloc #info.
        $info = $('#info'),
        info_h = $info.height(),
        info_w = $info.width(),
        info_pos = $info.offset(),
        info_top = info_pos.top,
        info_left = info_pos.left,
        // Géométrie du bloc #designer.
        $designer = $('#designer'),
        designer_w = $designer.width(),
        // Géométrie de la fenêtre.
        window_h = $(window).height();

      //$info.css('position', 'static');
      //$(window).unbind(scroll);

      var sticky_style = { position: 'fixed', 'background-color': '#fff' };

      if (window_h >= info_h + info_top) {
        // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
        // pour qu'il soit toujours visible on lui donne une position fixe.
        sticky_style.left = info_left + 'px';
        sticky_style.top = info_top + 'px';
        $info.css(sticky_style);
      }
      else if (window_h < info_h) {
        // La fenêtre est trop petite pour contenir tout le bloc info.
        // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
        // On ne touche donc à rien.
        return;
      }
      else {
        // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
        // haut de la fenêtre.
        var scroll_limit = info_top - 10,
          on = false;

        // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
        // plus tard lors d'un redimensionnement de la fenêtre.
        $info.css('left', info_left + 'px');

        sticky_style.top = '10px';

        if ($(window).scrollTop() >= scroll_limit) {
          on = true;
          $info.css(sticky_style);
        }

        $(window).scroll(function() {
          if ($(window).scrollTop() >= scroll_limit) {
            if (!on) {
              on = true;
              $info.css(sticky_style);
            }
          } else {
            if (on) {
              on = false;
              $info.css('position', 'static');
            }
          }
        });
      }

      $(window).resize(function() {
        // FIXME: On ne s'occupe pour le moment que des redimensionnements horizontaux.
        var left = $designer.offset().left + designer_w - info_w;

        $info.css({ 'left': left + 'px' });
      });
    }

    , _designer_common = function() {
      _designer_sticky_info();
    }
  ;

  /* jQuery plugins
   * ======================================================================= */

  $.fn.watermark = function(watermark) {
    if (watermark) {
      return this.each(function() {
        $(this).append('<div class=watermark><span>' + _(watermark) + '</span></div>');
      });
    }
    else {
      // Si aucun texte n'est fourni, on utilise l'attribut data-watermark.
      return this.each(function() {
        var $this = $(this);
        $this.append('<div class=watermark><span>' + $this.data('watermark') + '</span></div>');
      });
    }
  };

  /* Chiffon object
   * ======================================================================= */

  chiffon.config = function(options) {
    options = $.extend({}, chiffon.config.defaults, options);

    // En priorité, on utilise la langue définie dans la déclaration HTML, sinon on utilise
    // celle qui est précisée dans la configuration.
    chiffon.locale($('html').attr('lang') || options.defaultLocale);

    // Configuration globale du comportement des appels Ajax.
    chiffon.ajaxSetup(options.ajaxTimeout);

    return chiffon;
  };

  chiffon.config.defaults = {
    ajaxTimeout: 3000
    , defaultLocale: 'fr'
  };

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

  /* UI
   * ======================================================================= */

  var ui = chiffon.ui = {};

  ui.init = function() {
    // Les liens externes marqués par l'attribut rel=external s'ouvrent dans une nouvelle fenêtre.
    $('A[rel=external]').click(function() {
      window.open(this.href);
      return false;
    });

    //ui.stickyHeader.init();
    //ui.ajaxStatus();
    //ui.overlay.init();
    //ui.modal.init();

    //if (visitor.anonymous) {
    //  makeModal('register');

    //  //$.get('modal/register.html', function(data) { $modal.html(data); });
    //  //$modal.appendTo('BODY');

    //  $('A[rel~=modal]').click(function(e) {
    //    e.preventDefault();

    //    chiffon.ui.modal.register.show();

    //    // TODO: Use the Deferred jqXHR?
    //    $.ajax({
    //      type: 'GET'
    //      , global: false
    //      , dataType: 'html'
    //      , url: this.href
    //      , success: function(data) {
    //        var response = $('<html />').html(data);
    //        $('.contact_register').html(response.find('#content').html());
    //        chiffon.ui.overlay.show();
    //      }
    //    });
    //  });
    //}
  };

  /* Routes
   * ======================================================================= */

  var routes = chiffon.routes = {};

  routes.home_index = function() {
    $('.vignette').watermark('%vignette.watermark');
  };

  routes.designer_index = _designer_common;
  routes.designer_pattern = _designer_common;
  routes.designer_category = _designer_common;

  return chiffon;

})(this, jQuery, chiffon);

//connected = undef !== $.cookie('auth')

//, visitor = {
//  connected: connected
//  , anonymous: !connected

//  , logOn: function() {
//    throw 'Not Implemented';
//  }

//  , logOff: function() {
//    this.connected = false;
//    this.anonymous = true;
//  }
//}

//function makeModal(name) {
//  var $modal = $('<div class="modal contact_register"></div>');
//  $modal.appendTo('BODY');

//  chiffon.ui.modal[name] = {
//    show: function() {
//      $modal.show();
//      //chiffon.ui.modal.init();
//      //$modal.css('margin-top', -$modal.height() / 2);
//      //$modal.css('margin-left', -$modal.width() / 2);
//    }
//  };
//};

//// En-tête fixe.
//ui.stickyHeader = {
//  init: function() {
//    var $header = $('HEADER'),
//      header_pos = $header.position().top + $header.height(),
//      header_content = $header.children().clone(),
//      $sticky_header = $('<div id=sticky_header></div>'),
//      on = false;

//    $sticky_header.appendTo('BODY');
//    header_content.appendTo($sticky_header);

//    // TODO: si à l'ouverture, on est déjà trop bas, afficher l'en-tête statique.
//    // FIXME: en clonant l'en-tête on duplique les IDs...

//    $(window).scroll(function() {
//      if ($(this).scrollTop() < header_pos) {
//        if (on) {
//          on = false;
//          $sticky_header.hide();
//        }
//      } else {
//        // On est en dessous de la position limite d'activation.
//        if (!on) {
//          on = true;
//          $sticky_header.fadeIn('fast');
//        }
//      }
//    });
//  }
//};

//ui.modal = {
//  init: function() {
//    $('.modal').bind("clickoutside", function(e) {
//      $(this).hide();
//      //var $this = $(this);
//      //if ($form.is(":visible")) {
//      //  $form.fadeOut();
//      //}
//      //$this.unbind("clickoutside");
//    });
//  }
//};

//ui.overlay = (function() {
//  var $overlay = $('<div class=overlay></div>')

//  return {
//    init: function() {
//      $overlay.appendTo('BODY');
//      //$overlay.height(screen.height);
//      //$overlay.width(screen.width);
//    }

//    , show: function() {
//      $overlay.fadeIn();
//    }

//    , hide: function() {
//      $overlay.hide();
//    }
//  };
//})();

// Create & configure the ajax status placeholder.
//ui.ajaxStatus = function() {
//  var $status = $('<div id=ajax_status></div>')
//    , error = false;

//  $status.appendTo('BODY');

//  $(document).ajaxStart(function() {
//    $status
//      .removeClass('error')
//      .text(_('%ajax.loading'))
//      .show();
//  }).ajaxStop(function() {
//    if (error) {
//      error = false;
//    } else {
//      //$status.text(_('%ajax.done')).fadeOut('slow');
//      $status.fadeOut('slow');
//    }
//  }).ajaxError(function(e, req) {
//    var message = _(0 == req.status ? '%ajax.temp_error' : '%ajax.fatal_error');

//    error = true;
//    $status
//      .text(message)
//      .addClass('error')
//      .show()
//      .fadeOut(5000);
//  });
//};
