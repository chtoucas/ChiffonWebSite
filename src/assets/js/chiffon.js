;

// TODO:
// - ajouter html5shiv.js
// - ajouter es5-shim.js
// https://github.com/kriskowal/es5-shim/
// http://stackoverflow.com/questions/12779565/comparing-popular-script-loaders-yepnope-requirejs-labjs-and-headjs
// http://benalman.com/code/projects/jquery-resize/docs/files/jquery-ba-resize-js.html
// http://stackoverflow.com/questions/4298612/jquery-how-to-call-resize-event-only-once-its-finished-resizing

(function(window, $, Chiffon, undef) {
  'use strict';

  var
    // L10N
    _ = function(_string_) { return _string_.toLocaleString(); }
  ;

  /* Plugins jQuery.
   * ======================================================================= */

  $.fn.watermark = function(watermark) {
    if (undef !== watermark) {
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

  /* Configuration de l'objet Chiffon.
   * ======================================================================= */

  var configure = Chiffon.configure = function(options) {
    var opts = $.extend({}, configure.defaults, options);

    // En priorité, on utilise la langue définie dans la déclaration HTML, sinon on utilise
    // celle qui est précisée dans la configuration.
    configure.locale($('html').attr('lang') || opts.defaultLocale);

    // Configuration globale du comportement des appels Ajax.
    configure.ajax(opts.ajaxTimeout);
  };

  configure.defaults = {
    ajaxTimeout: 3000
    , defaultLocale: 'fr'
  };

  // Configuration de la L10N.
  configure.locale = function(locale) {
    String.locale = locale;
  };

  // Configuration des appels Ajax via jQuery.
  configure.ajax = function(timeout) {
    $.ajaxSetup({
      timeout: timeout
      , async: true
      , cache: true
    });
  };

  /* Méthodes publiques de l'objet Chiffon.
   * ======================================================================= */

  Chiffon.handle = function(route, params) {
    Chiffon.initUI();

    if (Routes.hasOwnProperty(route)) {
      Routes[route](params);
    }
  };

  Chiffon.initUI = function() {
    // Les liens externes avec l'attribut rel=external s'ouvrent dans une nouvelle fenêtre.
    $('A[rel=external]').click(function() {
      window.open(this.href);
      return false;
    });
  };

  // Navigation entre les différentes vues d'un même motif.
  var initViews = Chiffon.initViews = function(options) {
    var
      // Liste des vues.
      $views
      // Lien 'Précédent'.
      , $prev
      // Lien 'Précédent' factice.
      , $prev_noop
      // Lien 'Suivant'.
      , $next
      // Lien 'Suivant' factice.
      , $next_noop
      // Conteneur HTML pour la position courante.
      , $pos

      // Nombre de vues.
      , length
      // On démarre à 1, ce qui semble plus naturel.
      , pos = 1
      // Pour ne pas avoir à jongler entre index et position, on initialise la liste 'selectors' avec 
      // un élément factice en début de tableau.
      , selectors = [undef]
    ;

    // > Actions sur l'objet $views <

    function showView(sel) {
      $views.hide();
      $(sel).fadeIn();
    }

    function showViewAt(i) {
      showView(selectors[i]);
    }

    // > Actions sur l'objet $pos <

    function updatePosition(i) {
      // On met à jour l'indicateur de position.
      $pos.html(i);
    }

    // > Actions sur les objets $prev et $prev_noop <

    function disablePreviousLink() {
      $prev.hide();
      $prev_noop.show();
    }

    function enablePreviousLink() {
      $prev.show();
      $prev_noop.hide();
    }

    function setPreviousLinkAt(i) {
      // NB: i représente la position courante.
      $prev.attr('href', selectors[i - 1]);
    }

    // > Actions sur les objets $next et $next_noop <

    function disableNextLink() {
      $next.hide();
      $next_noop.show();
    }

    function enableNextLink() {
      $next.show();
      $next_noop.hide();
    }

    function setNextLinkAt(i) {
      // NB: i représente la position courante.
      $next.attr('href', selectors[i + 1]);
    }

    // > Utilitaires <

    function startAt(i) {
      // TODO: Vérifier que le motif présenté par défaut est bien visible au démarrage.
      updatePosition(i);

      setPreviousLinkAt(i);
      enablePreviousLink();

      // Lien 'Suivant'.
      if (length === i) {
        // En dernière position, on désactive le lien 'Suivant'.
        disableNextLink();
      } else {
        setNextLinkAt(i);
      }
    }

    function goBackAt(i) {
      showViewAt(i);
      updatePosition(i);

      // Lien 'Précédent'.
      if (1 === i) {
        // En première position, on désactive le lien 'Précédent'.
        disablePreviousLink();
      } else {
        setPreviousLinkAt(i);
      }

      // Lien 'Suivant'.
      if (length !== i) {
        // Si on n'est pas en dernière position, on met à jour le lien 'Suivant'.
        setNextLinkAt(i);

        if (length - 1 === i) {
          // En avant-dernière position, on active le lien 'Suivant'.
          enableNextLink();
        }
      }
    }

    function goForthAt(i) {
      showViewAt(i);
      updatePosition(i);

      // Lien 'Précédent'.
      if (1 !== i) {
        // Si on n'est pas en première position, on met à jour le lien 'Précédent'.
        setPreviousLinkAt(i);

        if (2 === i) {
          // En deuxième position, on active le lien 'Précédent'.
          enablePreviousLink();
        }
      }

      // Lien 'Suivant'.
      if (length === i) {
        // En dernière position, on désactive le lien 'Suivant'.
        disableNextLink();
      } else {
        setNextLinkAt(i);
      }
    }

    function getSelector(obj) {
      // FIXME: prop ou attr ?
      return '#' + $(obj).prop('id');
    }

    // > Constructeur <

    function setupLinks() {
      $prev.click(function(e) {
        e.preventDefault();

        goBackAt(--pos);
      });
      $next.click(function(e) {
        e.preventDefault();

        goForthAt(++pos);
      });
    }

    function initialize(options) {
      var $nav
        , currentSel
        , callback;

      $views = $(options.viewsSel);
      length = $views.length;

      if (length <= 1) {
        // Si on a au plus une vue, pas besoin d'aller plus loin.
        return;
      }

      $nav = $(options.navSel);
      $prev = $nav.find('.prev');
      $prev_noop = $nav.find('.prev_noop');
      $next = $nav.find('.next');
      $next_noop = $nav.find('.next_noop');
      $pos = $nav.find('.pos');

      currentSel = options.currentSel;

      if (undef === currentSel) {
        callback = function() { selectors.push(getSelector(this)); };
      } else {
        callback = function(i) {
          var sel = getSelector(this);
          if (currentSel === sel) {
            // On recherche la position initiale.
            pos = i + 1;
          }
          selectors.push(sel);
        };
      }

      $views.each(callback);
    }

    return (function(options) {
      var opts = $.extend({}, initViews.defaults, options);

      initialize(opts);

      if (length > 1) {
        if (pos > 1) {
          startAt(pos);
        }
        setupLinks();
      }
    })(options);
  };

  initViews.defaults = {
    viewsSel: '.pattern'
    , navSel: '#nav_views'
    , currentSel: undef
  };

  var stickInfo = Chiffon.stickInfo = function(options) {
    var
      $info
      , $designer

      // Géométrie du bloc #info.
      , info_h
      , info_w
      , info_pos
      , info_top
      , info_left

      // Géométrie du bloc #designer.
      , designer_w

      // Géométrie de la fenêtre.
      , window_h
    ;

    function setupSmallBlock() {
      $info.addClass('sticky');
      $info.css({ top: info_top + 'px', left: info_left + 'px' });
    }

    function setupMediumBlock() {
      var scroll_limit = info_top - 23
        , sticky_class = 'sticky top'
        , is_sticky = false
        , stick = function() {
          if (is_sticky) { return; }
          is_sticky = true;
          $info.addClass(sticky_class);
        }
        , unstick = function() {
          if (!is_sticky) { return; }
          is_sticky = false;
          $info.removeClass(sticky_class);
        }
      ;

      // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
      // plus tard lors d'un redimensionnement horizontal de la fenêtre.
      $info.css('left', info_left + 'px');

      if ($(window).scrollTop() >= scroll_limit) {
        stick();
      }

      $(window).scroll(function() {
        $(window).scrollTop() >= scroll_limit ? stick() : unstick();
      });
    }

    function handleResizeEvent() {
      $(window).resize(function() {
        // FIXME: Pour le moment, on ne s'occupe que des redimensionnements horizontaux.
        var left = $designer.offset().left + designer_w - info_w;

        $info.css({ 'left': left + 'px' });
      });
    }

    function initialize(options) {
      $info = $(options.infoSel);
      $designer = $(options.designerSel);
      info_h = $info.height();
      info_w = $info.width();
      info_pos = $info.offset();
      info_top = info_pos.top;
      info_left = info_pos.left;
      // Géométrie du bloc #designer.
      designer_w = $designer.width();
      // Géométrie de la fenêtre.
      window_h = $(window).height();
    }

    return (function(options) {
      var opts = $.extend({}, stickInfo.defaults, options);

      initialize(opts);

      if (window_h < info_h) {
        // La fenêtre est trop petite pour contenir tout le bloc info.
        // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
        // On ne touche donc à rien.
        return;
      } else if (window_h >= info_h + info_top) {
        // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
        // pour qu'il soit toujours visible on lui donne une position fixe.
        setupSmallBlock();
      } else {
        // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
        // haut de la fenêtre.
        setupMediumBlock();
      }

      handleResizeEvent();
    })(options);
  };

  stickInfo.defaults = {
    infoSel: '#info'
    , designerSel: '#designer'
  };

  /* Routes
   * ======================================================================= */

  var Routes = Chiffon.Routes = {}

  Routes.home_index = function() {
    $('.vignette').watermark('%vignette.watermark');
  };

  Routes.designer_index = Chiffon.stickInfo;
  Routes.designer_category = Chiffon.stickInfo;

  Routes.designer_pattern = function() {
    // NB: window.location.hash contient le caractère '#'.
    Chiffon.initViews({ currentSel: window.location.hash });
    Chiffon.stickInfo();
  };

})(this, jQuery, Chiffon);

//UI.stickyHeader.init();
//UI.ajaxStatus();
//UI.overlay.init();
//UI.modal.init();

//if (visitor.anonymous) {
//  makeModal('register');

//  //$.get('modal/register.html', function(data) { $modal.html(data); });
//  //$modal.appendTo('BODY');

//  $('A[rel~=modal]').click(function(e) {
//    e.preventDefault();

//    Chiffon.UI.modal.register.show();

//    // TODO: Use the Deferred jqXHR?
//    $.ajax({
//      type: 'GET'
//      , global: false
//      , dataType: 'html'
//      , url: this.href
//      , success: function(data) {
//        var response = $('<html />').html(data);
//        $('.contact_register').html(response.find('#content').html());
//        Chiffon.UI.overlay.show();
//      }
//    });
//  });
//}
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

//  Chiffon.UI.modal[name] = {
//    show: function() {
//      $modal.show();
//      //Chiffon.UI.modal.init();
//      //$modal.css('margin-top', -$modal.height() / 2);
//      //$modal.css('margin-left', -$modal.width() / 2);
//    }
//  };
//};

//// En-tête fixe.
//UI.stickyHeader = {
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

//UI.modal = {
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

//UI.overlay = (function() {
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
//UI.ajaxStatus = function() {
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
