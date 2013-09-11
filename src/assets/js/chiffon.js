;

// TODO:
// - ajouter html5shiv.js
// - utiliser es5-shim.js à la place d'ecma-5.js ?
// - https://github.com/kriskowal/es5-shim/
// - http://benalman.com/code/projects/jquery-resize/docs/files/jquery-ba-resize-js.html
// - http://stackoverflow.com/questions/4298612/jquery-how-to-call-resize-event-only-once-its-finished-resizing

(function(win, $, undef) {
  'use strict';

  $.fn.external = function() {
    return this.each(function() {
      $(this).click(function() {
        win.open(this.href);
        return false;
      });
    });
  };

  $.fn.watermark = function(watermark, options) {
    var settings = $.extend({}, $.fn.watermark.defaults, options);

    var getWatermak = undef !== watermark
      ? function($elt) { return watermark; }
      // Si aucun texte n'est fourni, on utilise la valeur de l'attribut 'data-watermark'.
      : function($elt) { return $elt.data('watermark'); };

    return this.each(function() {
      var $this = $(this);
      $this.append(settings.wrapperStart + getWatermak($this) + settings.wrapperEnd);
    });
  };

  $.fn.watermark.defaults = {
    wrapperStart: '<div class=watermark><span>'
    , wrapperEnd: '</span></div>'
  };

})(this, this.jQuery);

this.Chiffon = (function(win, doc, loc, _, $, undef) {
  'use strict';

  // Configuration par défaut.
  var defaults = {
    ajaxTimeout: 3000
  };

  // Configuration de L10N.
  function setLocale(locale) {
    String.locale = locale;
  }

  // Configuration globale du comportement des appels Ajax via jQuery.
  function setupAjax(timeout) {
    $.ajaxSetup({
      timeout: timeout
      , async: true
      , cache: true
    });
  }

  function breakFrame() {
    var top = win.top;

    if (top !== win) {
      if (undef !== loc) {
        top.location.replace(loc.href);
      }
      else {
        top.document.location.replace(doc.location.href);
      }
    }
  }

  function Chiffon(env, deps, options) {
    this.settings = _.defaults(options || {}, defaults);

    this.env = env;
    this.deps = deps;
  }

  Chiffon.prototype = {
    handleCore: function(controllerName, actionName, params) {
      throw new Error('You must override the "handleCore" method.');
    }

    , handle: function(controllerName, actionName, params) {
      breakFrame();
      setupAjax(this.settings.ajaxTimeout);
      setLocale(this.env.locale);

      this.handleCore(controllerName, actionName, params);
    }
  };

  return Chiffon;

})(this, this.document, this.location, this._, this.jQuery);

this.Chiffon.Presenters = (function($, undef) {
  'use strict';

  var Presenters = {};

  Presenters.Modal = (function() {
    function Modal(view) {
      this.view = view;
    }

    Modal.prototype = {
      loadContent: function(sender, e) {
        var that = this, href = e.href;

        // TODO: Use the Deferred jqXHR?
        $.ajax({
          type: 'GET'
          , dataType: 'html'
          , url: href
          , success: function(data) {
            that.view.onContentLoaded({ href: href, data: data });
          }
        });
      }
    };

    return Modal;
  })();

  /*
  Presenters.StickyInfo = (function() {
    // Configuration par défaut.
    var defaults = {
      stickyClass: 'sticky top'
    };

    var
      SMALL_SIZE = 1
      , MEDIUM_SIZE = 2
      , LARGE_SIZE = 3

      , size = SMALL_SIZE

      // Géométrie du bloc #info.
      , info_h
      , info_w
      , info_top
      , info_left

      // Géométrie du bloc #designer.
      , designer_w

      // Géométrie de la fenêtre.
      , window_h
    ;

    function StickyInfo(view, options) {
      this.$info = view.$info;
      this.$designer = view.$designer;
      this.size = LARGE_SIZE;
      this.settings = $.extend({}, defaults, options);
    }

    StickyInfo.prototype = {
      is_sticky: false

      , stick: function() {
        if (this.is_sticky) { return; }
        this.is_sticky = true;
        this.$info.addClass(this.settings.stickyClass);
      }

      , unstick: function() {
        if (!this.is_sticky) { return; }
        this.is_sticky = false;
        this.$info.removeClass(this.settings.stickyClass);
      }

      // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
      // pour qu'il soit toujours visible on lui donne une position fixe.
      , setupSmallBlock: function() {
        this.$info.addClass('sticky');
        this.$info.css({ top: info_top + 'px', left: info_left + 'px' });
      }

      // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
      // haut de la fenêtre.
      , setupMediumBlock: function() {
        // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
        // plus tard lors d'un redimensionnement horizontal de la fenêtre.
        this.$info.css('left', info_left + 'px');

        //handleScrollEventForMediumBlock();
      }

      , setup: function() {
        if (SMALL_SIZE === this.size) {
          this.setupSmallBlock();
        } else if (MEDIUM_SIZE === this.size) {
          this.setupMediumBlock();
        } else {
          ;
        }
      }

      , onDocumentReady: function(sender, e, callback) {
        info_h = this.$info.height();
        info_w = this.$info.width();

        info_offset = this.$info.offset();
        info_top = info_offset.top;
        info_left = info_offset.left;

        designer_w = this.$designer.width();
        window_h = $(win).height();

        if (window_h >= info_h + info_top) {
          this.size = SMALL_SIZE;
        } else if (window_h >= info_h) {
          this.size = MEDIUM_SIZE;
        } else {
          // La fenêtre est trop petite pour contenir tout le bloc info.
          // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
          // On ne touche donc à rien.
          this.size = LARGE_SIZE;
        }

        setup();

        callback(type);
      }

      // FIXME: Pour le moment, on ne s'occupe que des redimensionnements horizontaux.
      , onWindowResize: function(sender, e) {
        var left = this.$designer.offset().left + designer_w - info_w;

        this.$info.css({ 'left': left + 'px' });
      }

      , onWindowScroll: function(sender, e) {
        $(win).scrollTop() >= scroll_limit ? stick() : unstick();
      }
    };

    return StickyInfo;
  })();
  */

  return Presenters;
})(this.jQuery);

this.Chiffon.Views = (function(win, doc, loc, _, $, Chiffon, Presenters, undef) {
  'use strict';

  var Views = {};

  // L10N
  function l(string) {
    return string.toLocaleString();
  }

  /* Layouts */

  Views.Layout = (function() {
    function Layout() {
      //this.modalView = new Views.Modal();
      this.registerModalView = new Views.Modal({ linkSel: 'A[rel~=register]' });
      this.ajaxStatusView = new Views.AjaxStatus({ displayLoading: false });
    }

    Layout.prototype = {
      initialize: function() {
        //this.modalView.registerEventHandlers();
        this.registerModalView.registerEventHandlers();
        this.ajaxStatusView.registerEventHandlers();

        $('A[rel=external]').external();
      }
    };

    return Layout;
  })();

  Views.DesignerLayout = (function() {
    function DesignerLayout() {
      this.layoutView = new Views.Layout();
    }

    DesignerLayout.prototype = {
      initialize: function() {
        this.layoutView.initialize();
        Views.StickyInfo();
      }
    };

    return DesignerLayout;
  })();

  /* Pages */

  Views.ContactLogin = (function() {
    function ContactLogin() {
      this.layoutView = new Views.Layout();
    }

    ContactLogin.prototype = {
      initialize: function() {
        this.layoutView.initialize();
      }
    };

    return ContactLogin;
  })();

  Views.ContactNewsletter = (function() {
    function ContactNewsletter() {
      this.layoutView = new Views.Layout();
    }

    ContactNewsletter.prototype = {
      initialize: function() {
        this.layoutView.initialize();
      }
    };

    return ContactNewsletter;
  })();

  Views.ContactRegister = (function() {
    function ContactRegister() {
      this.layoutView = new Views.Layout();
    }

    ContactRegister.prototype = {
      initialize: function() {
        this.layoutView.initialize();
      }
    };

    return ContactRegister;
  })();

  Views.DesignerPattern = (function() {
    function DesignerPattern() {
      this.layoutView = new Views.DesignerLayout();
    }

    DesignerPattern.prototype = {
      initialize: function() {
        this.layoutView.initialize();

        // NB: location.hash contient le caractère '#'.
        Views.ViewNavigator({ currentSel: loc.hash });
      }
    };

    return DesignerPattern;
  })();

  Views.HomeIndex = (function() {
    function HomeIndex() {
      this.layoutView = new Views.Layout();
    }

    HomeIndex.prototype = {
      initialize: function() {
        this.layoutView.initialize();

        $('.vignette').watermark(l('%preview.watermark'));
      }
    };

    return HomeIndex;
  })();

  /* Composants communs */

  // Navigation entre les vues d'un même motif.
  Views.ViewNavigator = (function() {
    var settings
      , defaults = {
        viewsSel: '.pattern'
        , navSel: '#nav_views'
        , currentSel: undef
      };

    var
      // Liste des vues.
      $views
      // Lien 'Précédent'.
      , $prev
      // Lien 'Précédent' désactivé.
      , $prev_noop
      // Lien 'Suivant'.
      , $next
      // Lien 'Suivant' désactivé.
      , $next_noop
      // Conteneur HTML de la position courante.
      , $pos

      // Numéro de la vue courante. NB: On démarre à 1, ce qui semble plus naturel.
      , pos = 1
      // Nombre de vues.
      , length
      // Liste des sélecteurs de vue.
      // Pour ne pas avoir à jongler entre index et position, on initialise la liste
      // avec un élément factice en début de tableau.
      , selectors = [undef]
    ;

    /* Actions sur l'objet $views */

    // Rend visible la vue à la position 'i' et cache les autres vues.
    function showViewAt(i) {
      $views.hide();
      $(selectors[i]).fadeIn();
    }

    /* Actions sur l'objet $pos */

    // Met à jour l'indicateur visuel de position quand on est à la position 'i'.
    function setPositionMarkAt(i) {
      $pos.html(i);
    }

    /* Actions sur les objets $prev et $prev_noop */

    // Désactive le lien 'Précédent'.
    function disablePreviousLink() {
      $prev.hide();
      $prev_noop.show();
    }

    // Active le lien 'Précédent'.
    function enablePreviousLink() {
      $prev.show();
      $prev_noop.hide();
    }

    // Met à jour le lien 'Précédent' quand on arrive à la position 'i'.
    function setPreviousLinkAt(i) {
      $prev.attr('href', selectors[i - 1]);
    }

    /* Actions sur les objets $next et $next_noop */

    // Désactive le lien 'Suivant'.
    function disableNextLink() {
      $next.hide();
      $next_noop.show();
    }

    // Active le lien 'Suivant'.
    function enableNextLink() {
      $next.show();
      $next_noop.hide();
    }

    // Met à jour le lien 'Suivant' quand on arrive à la position 'i'.
    function setNextLinkAt(i) {
      $next.attr('href', selectors[i + 1]);
    }

    /* Utilitaires */

    function getSelector(obj) {
      // FIXME: prop ou attr ?
      return '#' + $(obj).prop('id');
    }

    function goTo(i) {
      showViewAt(i);
      setPositionMarkAt(i);
    }

    /* Fonctions utilisées lors d'un changement de position */

    // Actions à mener lors d'un démarrage à la position 'i'.
    function startAt(i) {
      // TODO: Vérifier que le motif présenté par défaut est bien visible au démarrage
      // et, donc, qu'on n'ait pas besoin d'appeler showViewAt(i).
      setPositionMarkAt(i);

      // On active le lien 'Précédent' car, au démarrage, il est invisible.
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

    // Actions à mener quand on avance à la position 'i'.
    function goBackTo(i) {
      goTo(i);

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

    // Actions à mener quand on recule à la position 'i'.
    function goForthTo(i) {
      goTo(i);

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

    /* Configuration et initialisation */

    function setupLinkHandlers() {
      $prev.click(function(e) {
        e.preventDefault();

        goBackTo(--pos);
      });
      $next.click(function(e) {
        e.preventDefault();

        goForthTo(++pos);
      });
    }

    function initialize() {
      var $nav
        , currentSel
        , callback;

      $views = $(settings.viewsSel);
      length = $views.length;

      if (length <= 1) {
        // Si on a au plus une vue, pas besoin d'aller plus loin.
        return;
      }

      $nav = $(settings.navSel);
      $prev = $nav.find('.prev');
      $prev_noop = $nav.find('.prev_noop');
      $next = $nav.find('.next');
      $next_noop = $nav.find('.next_noop');
      $pos = $nav.find('.pos');

      currentSel = settings.currentSel;

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

    return function(options) {
      settings = _.defaults(options || {}, defaults);

      initialize();

      if (length <= 1) { return; }

      if (pos > 1) {
        startAt(pos);
      }
      setupLinkHandlers();
    }
  })();

  // Autant que possible on s'assure que le bloc informations sur le designer est toujours visible.
  Views.StickyInfo = (function() {
    // Configuration par défaut.
    var settings
      , defaults = {
        infoSel: '#info'
        , designerSel: '#designer'
      };

    var $info
      , $designer

      // Géométrie du bloc #info.
      , info_h
      , info_w
      , info_top
      , info_left

      // Géométrie du bloc #designer.
      , designer_w

      // Géométrie de la fenêtre.
      , window_h
    ;

    function handleScrollEventForMediumBlock() {
      var scroll_limit
        , sticky_class = 'sticky top'
        , is_sticky = false;

      function stick() {
        if (is_sticky) { return; }
        is_sticky = true;
        $info.addClass(sticky_class);
      }

      function unstick() {
        if (!is_sticky) { return; }
        is_sticky = false;
        $info.removeClass(sticky_class);
      }

      scroll_limit = info_top - 23;

      if ($(win).scrollTop() >= scroll_limit) { stick(); }

      $(win).scroll(function() {
        $(win).scrollTop() >= scroll_limit ? stick() : unstick();
      });
    }

    // FIXME: Pour le moment, on ne s'occupe que des redimensionnements horizontaux.
    function handleResizeEvent() {
      $(win).resize(function() {
        var left = $designer.offset().left + designer_w - info_w;

        $info.css({ 'left': left + 'px' });
      });
    }

    // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
    // pour qu'il soit toujours visible on lui donne une position fixe.
    function setupSmallBlock() {
      $info.addClass('sticky');
      $info.css({ top: info_top + 'px', left: info_left + 'px' });
    }

    // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
    // haut de la fenêtre.
    function setupMediumBlock() {
      // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
      // plus tard lors d'un redimensionnement horizontal de la fenêtre.
      $info.css('left', info_left + 'px');

      handleScrollEventForMediumBlock();
    }

    function initialize() {
      var info_offset;

      $info = $(settings.infoSel);
      $designer = $(settings.designerSel);

      info_h = $info.height();
      info_w = $info.width();

      info_offset = $info.offset();
      info_top = info_offset.top;
      info_left = info_offset.left;

      designer_w = $designer.width();
      window_h = $(win).height();
    }

    return function(options) {
      settings = _.defaults(options || {}, defaults);

      initialize();

      if (window_h >= info_h + info_top) {
        setupSmallBlock();
      } else if (window_h >= info_h) {
        setupMediumBlock();
      } else {
        // La fenêtre est trop petite pour contenir tout le bloc info.
        // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
        // On ne touche donc à rien.
        return;
      }

      handleResizeEvent();
    }
  })();

  // En-tête de la page en position fixe.
  Views.StickyHeader = (function() {
    var settings
      , defaults = {
        headerSel: 'HEADER'
        , headerWrapper: '<div id=sticky_header></div>'
      };

    var $sticky_header
      , scroll_limit
      , is_sticky = false;

    function stick() {
      if (is_sticky) { return; }
      is_sticky = true;
      $sticky_header.fadeIn('fast');
    }

    function unstick() {
      if (!is_sticky) { return; }
      is_sticky = false;
      $sticky_header.hide();
    }

    function addStickyHeaderToDom() {
      $sticky_header.appendTo('BODY');
    }

    function handleScrollEvent() {
      if ($(win).scrollTop() >= scroll_limit) { stick(); }

      $(win).scroll(function() {
        $(win).scrollTop() >= scroll_limit ? stick() : unstick();
      });
    }

    function initialize() {
      var $header = $(settings.headerSel);

      // FIXME: en clonant l'en-tête on duplique les IDs...
      $sticky_header = $(settings.headerWrapper);
      $header.children().clone().appendTo($sticky_header);

      scroll_limit = $header.position().top + $header.height();
    }

    return function(options) {
      settings = _.defaults(options || {}, defaults);

      initialize();

      addStickyHeaderToDom();
      handleScrollEvent();
    }
  })();

  // Indicateur d'activité Ajax à la Google Mail.
  Views.AjaxStatus = (function() {
    // Configuration par défaut.
    var defaults = {
      errorClass: 'ajax_error'
      , displayLoading: true
      , onErrorFadingSpeed: 5000
      , loadingMessage: l('%ajax.loading')
      , notFoundErrorMessage: l('%ajax.notfound_error')
      , tempErrorMessage: l('%ajax.temp_error')
      , fatalErrorMessage: l('%ajax.fatal_error')
    };

    // Membres statiques.
    var $status, visible = false;

    // Constructeur.
    function AjaxStatus(options) {
      this.settings = $.extend({}, defaults, options);

      initialize({}, false /* initializing */);

      this.isError = false;
    }

    // Constructeur statique.
    var initialize = (function() {
      var defaults = { statusElement: '<div id=ajax_status></div>' };
      var initialized = false;

      return function(options, initializing) {
        if (initialized) {
          if (initializing) {
            throw new TypeError('You can not initialize "AjaxStatus" twice.');
          }
          else { return; }
        }

        var settings = _.defaults(options || {}, defaults);

        $status = $(settings.statusElement);

        // On ajoute l'élément status au DOM.
        $(doc.body).append($status);

        initialized = true;
      }
    })();

    AjaxStatus.AjaxStatus = function(options) { initialize(options, true /* initializing */); };

    AjaxStatus.prototype = {
      registerEventHandlers: function() {
        var that = this;

        $(doc)
          .ajaxStart(function(e) { that.onStart(e); })
          .ajaxError(function(e, req) { that.onError(e, req); })
          .ajaxStop(function(e) { that.onStop(e); });
      }

      , getErrorMessage: function(status) {
        if (404 === status) {
          return this.settings.notFoundErrorMessage;
        } else if (0 === status) {
          return this.settings.tempErrorMessage;
        } else {
          return this.settings.fatalErrorMessage;
        }
      }

      , onStart: function(e) {
        // On remet les compteurs à zéro.
        $status.removeClass(this.settings.errorClass);

        if (this.settings.displayLoading) {
          $status.text(this.settings.loadingMessage).show();
        }
      }

      , onStop: function(e) {
        if (this.isError) {
          this.isError = false;
        } else if (this.settings.displayLoading) {
          $status.hide();
        }
      }

      , onError: function(e, req) {
        var fadingOutSpeed = this.settings.onErrorFadingOutSpeed
          , message = this.getErrorMessage(req.status);

        this.isError = true;
        $status
          .addClass(this.settings.errorClass)
          .text(message)
          .show();

        if (fadingOutSpeed > 0) {
          $status.fadeOut(fadingOutSpeed);
        }
      }
    };

    return AjaxStatus;
  })();

  Views.Modal = (function() {
    // Configuration par défaut.
    var defaults = {
      linkSel: 'A[rel~=modal]'
    };

    var ESC_KEYCODE = 27;
    var $overlay, $modal, lastHref = '', opened = false;

    function Modal(options) {
      this.settings = _.defaults(options || {}, defaults);

      initialize({}, false /* initializing */);

      this.presenter = new Presenters.Modal(this);
    }

    var initialize = (function() {
      var defaults = {
        modalElement: '<div class=modal></div>'
        , overlayElement: '<div class=overlay></div>'
      };
      var initialized = false;

      return function(options, initializing) {
        if (initialized) {
          if (initializing) {
            throw new TypeError('You can not initialize "Modal" twice.');
          }
          else { return; }
        }

        var settings = $.extend({}, defaults, options);

        $overlay = $(settings.overlayElement);
        $modal = $(settings.modalElement);

        // On ajoute les éléments au DOM.
        $(doc.body).append($overlay).append($modal);

        // On enregistre les événements associés.
        $(doc).bind('keydown.modal', function(e) { keydownPressed(this, e); });

        $overlay.click(function(e) { overlayClicked(this, e); });

        initialized = true;
      }
    })();

    function open() {
      $overlay.show();
      $modal.show();
      opened = true;
    }

    function close() {
      $modal.hide();
      $overlay.fadeOut();
      opened = false;
    }

    function keydownPressed(sender, e) {
      if (opened && ESC_KEYCODE === e.keyCode) {
        e.preventDefault();
        close();
      }
    }

    function overlayClicked(sender, e) {
      if (opened) {
        e.preventDefault();
        close();
      }
    }

    Modal.Modal = function(options) { initialize(options, true /* initializing */); };

    Modal.prototype = {
      registerEventHandlers: function() {
        var that = this;

        $(this.settings.linkSel).click(function(e) { that.linkClicked(this, e); });
      }

      , linkClicked: function(sender, e) {
        e.preventDefault();

        var href = sender.href;

        if (lastHref === href) {
          open();
        } else {
          this.onLoadContent({ href: href });
        }
      }

      , loadContent: function(sender, e) {
        this.presenter.loadContent(sender, e);
      }

      , onLoadContent: function(e) {
        this.loadContent(this, e);
      }

      , onContentLoaded: function(e) {
        $modal.html(e.data);
        open();
        lastHref = e.href;
      }
    };

    return Modal;
  })();

  return Views;

})(this, this.document, this.location, this._, this.jQuery, this.Chiffon, this.Chiffon.Presenters);

this.Chiffon.Controllers = (function($, Views, undef) {
  'use strict';

  function extend(methods) { return $.extend({}, BaseController.prototype, methods); }

  /* BaseController */

  function BaseController(env, deps) {
    this.user = env.user;
    this.deps = deps;
  }

  /* ContactController */

  function ContactController() {
    BaseController.apply(this, arguments);
  }

  ContactController.prototype = extend({
    login: function() { (new Views.ContactLogin()).initialize(); }
    , newsletter: function() { (new Views.ContactRegister()).initialize(); }
    , register: function() { (new Views.ContactNewsletter()).initialize(); }
  });

  /* DesignerController */

  function DesignerController() {
    BaseController.apply(this, arguments);
  }

  DesignerController.prototype = extend({
    index: function() { (new Views.DesignerLayout()).initialize(); }
    , category: function() { (new Views.DesignerLayout()).initialize(); }
    , pattern: function() { (new Views.DesignerPattern()).initialize(); }
  });

  /* HomeController */

  function HomeController() {
    BaseController.apply(this, arguments);
  }

  HomeController.prototype = extend({
    about: function() { (new Views.Layout()).initialize(); }
    , contact: function() { (new Views.Layout()).initialize(); }
    , index: function() { (new Views.HomeIndex()).initialize(); }
  });

  return {
    Contact: ContactController
    , Designer: DesignerController
    , Home: HomeController
  };

})(this.jQuery, this.Chiffon.Views);

this.Chiffon.prototype.handleCore = (function(Controllers, undef) {
  'use strict';

  return function(controllerName, actionName, params) {
    if (Controllers.hasOwnProperty(controllerName)) {
      var controllerClass = Controllers[controllerName]
        , controllerPrototype = controllerClass.prototype
        , actionMethod = actionName.toLowerCase();

      if (controllerPrototype.hasOwnProperty(actionMethod)) {
        var controller = new controllerClass(this.env, this.deps);
        controllerPrototype[actionMethod].apply(controller, params);
      }
    }
  };

})(this.Chiffon.Controllers);
