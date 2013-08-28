;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
//    https://github.com/kriskowal/es5-shim/
// http://stackoverflow.com/questions/12779565/comparing-popular-script-loaders-yepnope-requirejs-labjs-and-headjs

(function(window, $, chiffon, undef) {
  'use strict';

  var
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

     // L10N
    _ = function(string) { return string.toLocaleString(); }

    // TODO:
    // http://benalman.com/code/projects/jquery-resize/docs/files/jquery-ba-resize-js.html
    // http://stackoverflow.com/questions/4298612/jquery-how-to-call-resize-event-only-once-its-finished-resizing
    , _sticky_designer_info = function() {
      var $info = $('#info'),
        info_h = $info.height(),
        info_w = $info.width(),
        info_pos = $info.offset(),
        info_top = info_pos.top,
        $designer = $('#designer'),
        designer_w = $designer.width(),
        window_h = $(window).height();

      //$info.css('position', 'static');
      //$(window).unbind(scroll);
      // TODO: background color

      if (window_h >= info_h + info_top) {
        // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ; 
        // on lui donne une position fixe.
        $info.css('position', 'fixed');
        $info.css('top', info_top + 'px');
        $info.css('left', info_pos.left + 'px');
      }
      else if (window_h < info_h) {
        // La fenêtre est trop petite pour contenir tout le bloc info, on ne touche donc à rien,
        // sinon le bas du bloc info ne sera jamais visible.
        return;
      }
      else {
        // La fenêtre peut contenir tout le bloc info si on ne le laisse pas dans sa position initiale.
        var css = { position: 'fixed', top: '10px', left: info_pos.left + 'px' },
          on = false;
        if ($(window).scrollTop() >= info_top - 10) { $info.css(css); }
        $(window).scroll(function() {
          if ($(this).scrollTop() >= info_top - 10) {
            if (!on) {
              on = true;
              $info.css(css);
            }
          } else {
            if (on) {
              on = false;
              $info.css('position', 'static');
            }
          }
        });
      }

      // FIXME
      $(window).resize(function() {
        var left = $designer.offset().left + designer_w - info_w;

        $info.css({ 'left': left + 'px' });
      });
    }

    , _designer_common = function() {
      //ui.stickyHeader.init();

      _sticky_designer_info();
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

    // Pick up the locale from the HTML declaration and if not found use the default locale.
    chiffon.locale($('html').attr('lang') || options.defaultLocale);

    // Configure Ajax.
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
    // Open external links in a new window.
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
