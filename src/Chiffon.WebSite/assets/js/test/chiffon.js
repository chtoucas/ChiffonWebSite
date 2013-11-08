//this.Chiffon.Presenters = (function($, undef) {
//  'use strict';

//  var Presenters = {};

//  Presenters.Modal = (function() {
//    function Modal(view) {
//      this.view = view;
//    }

//    Modal.prototype = {
//      loadContent: function(sender, e) {
//        var that = this;
//        var href = e.href;

//        // TODO: Use the Deferred jqXHR?
//        $.ajax({
//          type: 'GET'
//          , dataType: 'html'
//          , url: href
//          , success: function(data) {
//            that.view.onContentLoaded({ href: href, data: data });
//          }
//        });
//      }
//    };

//    return Modal;
//  })();

//  return Presenters;
//})(this.jQuery);

//// En-tête de la page en position fixe.
//Views.StickyHeader = (function() {
//  var defaults = {
//    headerSel: 'HEADER'
//      , headerWrapper: '<div id=sticky_header></div>'
//  };

//  var settings;

//  var $sticky_header;
//  var scroll_limit;
//  var is_sticky = false;

//  function stick() {
//    if (is_sticky) { return; }
//    is_sticky = true;
//    $sticky_header.fadeIn('fast');
//  }

//  function unstick() {
//    if (!is_sticky) { return; }
//    is_sticky = false;
//    $sticky_header.hide();
//  }

//  function addStickyHeaderToDom() {
//    $sticky_header.appendTo('BODY');
//  }

//  function handleScrollEvent() {
//    if ($(window).scrollTop() >= scroll_limit) { stick(); }

//    $(window).scroll(function() {
//      $(window).scrollTop() >= scroll_limit ? stick() : unstick();
//    });
//  }

//  function initialize() {
//    var $header = $(settings.headerSel);

//    // FIXME: en clonant l'en-tête on duplique les IDs...
//    $sticky_header = $(settings.headerWrapper);
//    $header.children().clone().appendTo($sticky_header);

//    scroll_limit = $header.position().top + $header.height();
//  }

//  return function(options) {
//    settings = _.defaults(options || {}, defaults);

//    initialize();

//    addStickyHeaderToDom();
//    handleScrollEvent();
//  }
//})();

//// Indicateur d'activité Ajax à la Google Mail.
//Views.AjaxStatus = (function() {
//  // Configuration par défaut.
//  var defaults = {
//    errorClass: 'ajax_error'
//    , displayLoading: true
//    , onErrorFadingSpeed: 5000
//    , loadingMessage: l('%ajax.loading')
//    , notFoundErrorMessage: l('%ajax.notfound_error')
//    , tempErrorMessage: l('%ajax.temp_error')
//    , fatalErrorMessage: l('%ajax.fatal_error')
//  };

//  // Membres statiques.
//  var $status, visible = false;

//  // Constructeur.
//  function AjaxStatus(options) {
//    this.settings = $.extend({}, defaults, options);

//    initialize({}, false /* initializing */);

//    this.isError = false;
//  }

//  // Constructeur statique.
//  var initialize = (function() {
//    var defaults = { statusElement: '<div id=ajax_status></div>' };
//    var initialized = false;

//    return function(options, initializing) {
//      if (initialized) {
//        if (initializing) {
//          throw new TypeError('You can not initialize "AjaxStatus" twice.');
//        }
//        else { return; }
//      }

//      var settings = _.defaults(options || {}, defaults);

//      $status = $(settings.statusElement);

//      // On ajoute l'élément status au DOM.
//      $(document.body).append($status);

//      initialized = true;
//    }
//  })();

//  AjaxStatus.AjaxStatus = function(options) { initialize(options, true /* initializing */); };

//  AjaxStatus.prototype = {
//    registerEventHandlers: function() {
//      var that = this;

//      $(document)
//        .ajaxStart(function(e) { that.onStart(e); })
//        .ajaxError(function(e, req) { that.onError(e, req); })
//        .ajaxStop(function(e) { that.onStop(e); });
//    }

//    , getErrorMessage: function(status) {
//      if (404 === status) {
//        return this.settings.notFoundErrorMessage;
//      } else if (0 === status) {
//        return this.settings.tempErrorMessage;
//      } else {
//        return this.settings.fatalErrorMessage;
//      }
//    }

//    , onStart: function(e) {
//      // On remet les compteurs à zéro.
//      $status.removeClass(this.settings.errorClass);

//      if (this.settings.displayLoading) {
//        $status.text(this.settings.loadingMessage).show();
//      }
//    }

//    , onStop: function(e) {
//      if (this.isError) {
//        this.isError = false;
//      } else if (this.settings.displayLoading) {
//        $status.hide();
//      }
//    }

//    , onError: function(e, req) {
//      var fadingOutSpeed = this.settings.onErrorFadingOutSpeed;
//      var message = this.getErrorMessage(req.status);

//      this.isError = true;
//      $status
//        .addClass(this.settings.errorClass)
//        .text(message)
//        .show();

//      if (fadingOutSpeed > 0) {
//        $status.fadeOut(fadingOutSpeed);
//      }
//    }
//  };

//  return AjaxStatus;
//})();

//Views.Modal = (function() {
//  // Configuration par défaut.
//  var defaults = {
//    linkSel: 'A[rel~=modal]'
//  };

//  var ESC_KEYCODE = 27;
//  var $overlay;
//  var $modal;
//  var lastHref = '';
//  var opened = false;
//  var initialize;

//  function Modal(options) {
//    this.settings = _.defaults(options || {}, defaults);

//    initialize({}, false /* initializing */);

//    this.presenter = new Presenters.Modal(this);
//  }

//  initialize = (function() {
//    var defaults = {
//      modalElement: '<div class=modal></div>'
//      , overlayElement: '<div class=overlay></div>'
//    };
//    var initialized = false;

//    return function(options, initializing) {
//      if (initialized) {
//        if (initializing) {
//          throw new TypeError('You can not initialize "Modal" twice.');
//        }
//        else { return; }
//      }

//      var settings = $.extend({}, defaults, options);

//      $overlay = $(settings.overlayElement);
//      $modal = $(settings.modalElement);

//      // On ajoute les éléments au DOM.
//      $(document.body).append($overlay).append($modal);

//      // On enregistre les événements associés.
//      $(document).bind('keydown.modal', function(e) { keydownPressed(this, e); });

//      $overlay.click(function(e) { overlayClicked(this, e); });

//      initialized = true;
//    }
//  })();

//  function open() {
//    $overlay.show();
//    $modal.show();
//    opened = true;
//  }

//  function close() {
//    $modal.hide();
//    $overlay.fadeOut();
//    opened = false;
//  }

//  function keydownPressed(sender, e) {
//    if (opened && ESC_KEYCODE === e.keyCode) {
//      e.preventDefault();
//      close();
//    }
//  }

//  function overlayClicked(sender, e) {
//    if (opened) {
//      e.preventDefault();
//      close();
//    }
//  }

//  Modal.Modal = function(options) { initialize(options, true /* initializing */); };

//  Modal.prototype = {
//    registerEventHandlers: function() {
//      var that = this;

//      $(this.settings.linkSel).click(function(e) { that.linkClicked(this, e); });
//    }

//    , linkClicked: function(sender, e) {
//      e.preventDefault();

//      var href = sender.href;

//      if (lastHref === href) {
//        open();
//      } else {
//        this.onLoadContent({ href: href });
//      }
//    }

//    , loadContent: function(sender, e) {
//      this.presenter.loadContent(sender, e);
//    }

//    , onLoadContent: function(e) {
//      this.loadContent(this, e);
//    }

//    , onContentLoaded: function(e) {
//      $modal.html(e.data);
//      open();
//      lastHref = e.href;
//    }
//  };

//  return Modal;
//})();

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
      window_h = $(window).height();

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
      $(window).scrollTop() >= scroll_limit ? stick() : unstick();
    }
  };

  return StickyInfo;
})();
*/
