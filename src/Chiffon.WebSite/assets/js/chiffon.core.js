/*global Chiffon, _, jQuery*/

(function($, Chiffon) {
  'use strict';

  // Configuration par défaut.
  var defaults = {
    ajaxTimeout: 3000
  };

  function trace(controllerName, actionName) {
    if (DEBUG) {
      console.log('The requested action "' + controllerName + '.' + actionName + '" does not exist.');
    }
  }

  Chiffon.prototype = {
    init: function(options) {
      var settings = _.defaults(options || {}, defaults);

      // Configuration de L10N.
      String.locale = this.context.locale;

      // Configuration globale du comportement des appels Ajax via jQuery.
      $.ajaxSetup({
        timeout: settings.ajaxTimeout,
        async: true,
        cache: true
      });

      return this;
    },

    handle: function(controllerName, actionName, params) {
      var Controllers = Chiffon.Controllers;
      if (!Controllers.hasOwnProperty(controllerName)) {
        trace(controllerName, actionName);
        return;
      }

      var controller;
      var ControllerClass = Controllers[controllerName];
      var controllerPrototype = ControllerClass.prototype;

      if (!controllerPrototype.hasOwnProperty(actionName)) {
        trace(controllerName, actionName);
        return;
      }

      controller = new ControllerClass(this.context);
      controllerPrototype[actionName].apply(controller, params);

      return;
    }
  };

})(jQuery, Chiffon);

// Composants communs.
/*jshint -W074*/
Chiffon.Components = (function(window, $, undef) {
  'use strict';

  var Components = {};

  // Navigation entre les vues d'un même motif.
  /*jshint -W071*/
  Components.ViewNavigator = (function() {
    var settings;
    var defaults = {
      viewsSel: '.pattern',
      navSel: '#nav_views',
      currentSel: undef
    };

    // Liste des vues.
    var $views;
    // Lien 'Précédent'.
    var $prev;
    // Lien 'Précédent' désactivé.
    var $prevNoop;
    // Lien 'Suivant'.
    var $next;
    // Lien 'Suivant' désactivé.
    var $nextNoop;
    // Conteneur HTML de la position courante.
    var $pos;

    // Numéro de la vue courante. NB: On démarre à 1, ce qui semble plus naturel.
    var pos = 1;
    // Nombre de vues.
    var length;
    // Liste des sélecteurs de vue.
    // Pour ne pas avoir à jongler entre index et position, on initialise la liste
    // avec un élément factice en début de tableau.
    var selectors = [undef];

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

    /* Actions sur les objets $prev et $prevNoop */

    // Désactive le lien 'Précédent'.
    function disablePreviousLink() {
      $prev.hide();
      $prevNoop.show();
    }

    // Active le lien 'Précédent'.
    function enablePreviousLink() {
      $prev.show();
      $prevNoop.hide();
    }

    // Met à jour le lien 'Précédent' quand on arrive à la position 'i'.
    function setPreviousLinkAt(i) {
      $prev.attr('href', selectors[i - 1]);
    }

    /* Actions sur les objets $next et $nextNoop */

    // Désactive le lien 'Suivant'.
    function disableNextLink() {
      $next.hide();
      $nextNoop.show();
    }

    // Active le lien 'Suivant'.
    function enableNextLink() {
      $next.show();
      $nextNoop.hide();
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

    function init() {
      var $nav;
      var currentSel;
      var callback;

      $views = $(settings.viewsSel);
      length = $views.length;

      if (length <= 1) {
        // Si on a au plus une vue, pas besoin d'aller plus loin.
        return;
      }

      $nav = $(settings.navSel);
      $prev = $nav.find('.prev');
      $prevNoop = $nav.find('.prev_noop');
      $next = $nav.find('.next');
      $nextNoop = $nav.find('.next_noop');
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

      init();

      if (length <= 1) { return; }

      if (pos > 1) {
        startAt(pos);
      }
      setupLinkHandlers();
    };
  })();
  /*jshint +W071*/

  // Autant que possible on s'assure que le bloc informations sur le designer est toujours visible.
  Components.StickyInfo = (function() {
    // Configuration par défaut.
    var settings;
    var defaults = {
      infoSel: '#info',
      designerSel: '#designer'
    };

    var $info;
    var $designer;

    // Géométrie du bloc #info.
    var infoHeight;
    var infoWidth;
    var infoTop;
    var infoLeft;

    // Géométrie du bloc #designer.
    var designerWidth;

    // Géométrie de la fenêtre.
    var windowHeight;

    function handleScrollEventForMediumBlock() {
      var scrollLimit;
      var stickyClass = 'sticky top';
      var isSticky = false;

      function stick() {
        if (isSticky) { return; }
        isSticky = true;
        $info.addClass(stickyClass);
      }

      function unstick() {
        if (!isSticky) { return; }
        isSticky = false;
        $info.removeClass(stickyClass);
      }

      scrollLimit = infoTop - 23;

      if ($(window).scrollTop() >= scrollLimit) { stick(); }

      $(window).scroll(function() {
        if ($(window).scrollTop() >= scrollLimit) { stick(); } else { unstick(); }
      });
    }

    // FIXME: Pour le moment, on ne s'occupe que des redimensionnements horizontaux.
    function handleResizeEvent() {
      // On utilise "_.debounce()" pour temporiser la prise en charge de l'évènement "resize"
      // pendant 150 millisecondes.
      $(window).resize(_.debounce(function() {
        var left = $designer.offset().left + designerWidth - infoWidth;

        $info.css({ 'left': left + 'px' });
      }, 150));
    }

    // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
    // pour qu'il soit toujours visible on lui donne une position fixe.
    function setupSmallBlock() {
      $info.addClass('sticky');
      $info.css({ top: infoTop + 'px', left: infoLeft + 'px' });
    }

    // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
    // haut de la fenêtre.
    function setupMediumBlock() {
      // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
      // plus tard lors d'un redimensionnement horizontal de la fenêtre.
      $info.css('left', infoLeft + 'px');

      handleScrollEventForMediumBlock();
    }

    function init() {
      var infoOffset;

      $info = $(settings.infoSel);
      $designer = $(settings.designerSel);

      infoHeight = $info.height();
      infoWidth = $info.width();

      infoOffset = $info.offset();
      infoTop = infoOffset.top;
      infoLeft = infoOffset.left;

      designerWidth = $designer.width();
      windowHeight = $(window).height();
    }

    return function(options) {
      settings = _.defaults(options || {}, defaults);

      init();

      if (windowHeight >= infoHeight + infoTop) {
        setupSmallBlock();
      } else if (windowHeight >= infoHeight) {
        setupMediumBlock();
      } else {
        // La fenêtre est trop petite pour contenir tout le bloc info.
        // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
        // On ne touche donc à rien.
        return;
      }

      handleResizeEvent();
    };
  })();

  return Components;

})(this, jQuery);
/*jshint +W074*/

/*jshint -W072*/
Chiffon.Views = (function(window, $, Components, undef) {
  /*jshint +W072*/
  'use strict';

  var document = window.document;
  var location = window.location;
  var Views = {};

  // L10N
  function l(string) {
    return string.toLocaleString();
  }

  // Chargement de jQuery.validate puis exécution d'un callback.
  function initializeValidation(context, fn) {
    context.require(context.resources.validation, function() {
      var $errContainer = $('#error_container');

      $.validator.setDefaults({
        hightlight: function(elmt) { $(elmt).addClass('error'); },
        unhightlight: function(elmt) { $(elmt).removeClass('error'); },
        errorContainer: $errContainer,
        errorLabelContainer: $errContainer,
        invalidHandler: function() { $errContainer.fadeIn(); }
      });

      if (undef !== fn) { fn(); }
    });
  }

  /* BaseView */

  function BaseView(context) {
    this.context = context;
  }

  /* Layouts */

  Views.Layout = (function() {
    function Layout() {
      BaseView.apply(this, arguments);
    }

    Layout.prototype = {
      init: function() {
        // On ouvre les liens externes dans une nouvelle fenêtre.
        $('A[rel=external]').external();

        // Pour les visiteurs anonymes uniquement, on active les modales.
        if (!this.context.isAuth) {
          // NB: On place l'événement sur "document" car on veut rester dans la modale après un clic.
          $(document).on('click.modal', 'A[rel="modal:open"]', function(e) {
            e.preventDefault();

            $(this).modal({ closeText: l('%modal.close') });
          });
        } /* else {
          // TODO: Utiliser un hashcode pour afficher la confirmation de compte.
          //$('<div class="welcome serif serif_large"><h2>Bienvenue !</h2><p>Merci de vous être inscrit.</p></div>')
          //  .appendTo('body').modal({ closeText: l('%modal.close') });
        } */
      }
    };

    return Layout;
  })();

  Views.DesignerLayout = (function() {
    function DesignerLayout(context) {
      BaseView.apply(this, arguments);
      this.layoutView = new Views.Layout(context);
    }

    DesignerLayout.prototype = {
      init: function() {
        this.layoutView.init();
        Components.StickyInfo();
      }
    };

    return DesignerLayout;
  })();

  function DefaultView(context) {
    BaseView.apply(this, arguments);
    this.layoutView = new Views.Layout(context);
  }

  DefaultView.prototype = {
    init: function() {
      this.layoutView.init();
    }
  };

  function DesignerView(context) {
    BaseView.apply(this, arguments);
    this.layoutView = new Views.DesignerLayout(context);
  }

  DesignerView.prototype = {
    init: function() {
      this.layoutView.init();
    }
  };

  /* Pages Home */

  Views.HomeIndex = (function() {
    function HomeIndex() {
      DefaultView.apply(this, arguments);
    }

    HomeIndex.prototype = {
      init: function() {
        DefaultView.prototype.init.call(this);

        $('.vignette').watermark(l('%preview.watermark'));
      }
    };

    return HomeIndex;
  })();

  /* Pages Contact */

  Views.AccountLogin = (function() {
    function AccountLogin() {
      DefaultView.apply(this, arguments);
    }

    AccountLogin.prototype = {
      init: function() {
        DefaultView.prototype.init.call(this);

        initializeValidation(this.context, function() {
          $('#login_form').validate({
            messages: { token: l('%login.password_required') },
            rules: { token: { required: true, minlength: 10 } }
          });
        });
      }
    };

    return AccountLogin;
  })();

  Views.AccountRegister = (function() {
    function AccountRegister() {
      DefaultView.apply(this, arguments);
    }

    AccountRegister.prototype = {
      init: function() {
        DefaultView.prototype.init.call(this);

        var $form = $('#register_form');

        initializeValidation(this.context, function() {
          $form.validate({
            // NB: On ne veut pas de message d'erreur par "input".
            // TODO: "errorPlacement" ne semble pas être la bonne méthode à utiliser.
            errorPlacement: $.noop,
            messages: {
              Lastname: '',
              Firstname: '',
              CompanyName: '',
              EmailAddress: ''
            },
            rules: {
              Lastname: { required: true, minlength: 2, maxlength: 50 },
              Firstname: { required: true, minlength: 2, maxlength: 50 },
              CompanyName: { required: true, minlength: 2, maxlength: 100 },
              EmailAddress: { required: true, minlength: 5, maxlength: 200 },
              Message: { maxlength: 4000 }
            }
          });
        });
      }
    };

    return AccountRegister;
  })();

  /* Pages Designer */

  Views.DesignerPattern = (function() {
    function DesignerPattern() {
      DesignerView.apply(this, arguments);
    }

    DesignerPattern.prototype = {
      init: function() {
        DesignerView.prototype.init.call(this);

        // NB: location.hash contient le caractère '#'.
        Components.ViewNavigator({ currentSel: location.hash });
      }
    };

    return DesignerPattern;
  })();

  return Views;

})(this, jQuery, Chiffon.Components);

Chiffon.Controllers = (function($, Views) {
  'use strict';

  function extend(methods) { return $.extend({}, BaseController.prototype, methods); }

  /* BaseController */

  function BaseController(context) {
    this.context = context;
  }

  /* ContactController */

  function AccountController() {
    BaseController.apply(this, arguments);
  }

  AccountController.prototype = extend({
    Login: function() { (new Views.AccountLogin(this.context)).init(); },
    Newsletter: function() { (new Views.Layout(this.context)).init(); },
    Register: function() { (new Views.AccountRegister(this.context)).init(); }
  });

  /* DesignerController */

  function DesignerController() {
    BaseController.apply(this, arguments);
  }

  DesignerController.prototype = extend({
    Index: function() { (new Views.DesignerLayout(this.context)).init(); },
    Category: function() { (new Views.DesignerLayout(this.context)).init(); },
    Pattern: function() { (new Views.DesignerPattern(this.context)).init(); }
  });

  /* HomeController */

  function HomeController() {
    BaseController.apply(this, arguments);
  }

  HomeController.prototype = extend({
    About: function() { (new Views.Layout(this.context)).init(); },
    Contact: function() { (new Views.Layout(this.context)).init(); },
    Index: function() { (new Views.HomeIndex(this.context)).init(); }
  });

  return {
    Account: AccountController,
    Designer: DesignerController,
    Home: HomeController
  };

}(jQuery, Chiffon.Views));
