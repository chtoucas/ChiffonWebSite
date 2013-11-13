/*global Chiffon, _, $, ł*/

(function(window) {
  'use strict';

  // Utilitaire de localisation.
  window.ł = function(value) {
    return value.toLocaleString();
  };
})(this);

Chiffon.init = (function() {
  'use strict';

  // Configuration par défaut.
  var defaults = {
    ajaxTimeout: 3000
  };

  // Configuration globale de l'application.
  return function(options) {
    var settings = _.defaults(options || {}, defaults);

    // Comportement des appels Ajax via jQuery.
    $.ajaxSetup({
      timeout: settings.ajaxTimeout,
      async: true,
      cache: true
    });
  };

})();

// Composants communs.
/*jshint -W074*/
Chiffon.Components = (function(window, undef) {
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

})(this);
/*jshint +W074*/

Chiffon.Views = (function(window, undef) {
  'use strict';

  var document = window.document;
  var Views = {};

  Views.View = function(context) {
    this.context = context;

    this.init = function() { this.initLayout(); this.initCore(); };
    //this.initLayout = $.noop;
    //this.initCore = $.noop;
  };

  Views.ValidateMixin = {
    // Chargement de jQuery.validate puis exécution d'un callback.
    validate: function(fn) {
      this.context.require(this.context.resources.validation, function() {
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
  };

  Views.LayoutMixin = {
    initLayout: function() {
      // On ouvre les liens externes dans une nouvelle fenêtre.
      $('A[rel=external]').external();

      // Pour les visiteurs anonymes uniquement, on active les modales.
      if (!this.context.isAuth) {
        // NB: On place l'événement sur "document" car on veut rester dans la modale après un clic.
        $(document).on('click.modal', 'A[rel="modal:open"]', function(e) {
          e.preventDefault();

          $(this).modal({ closeText: ł('%modal.close') });
        });
      } /* else {
          // TODO: Utiliser un hashcode pour afficher la confirmation de compte.
          //$('<div class="welcome serif serif_large"><h2>Bienvenue !</h2><p>Merci de vous être inscrit.</p></div>')
          //  .appendTo('body').modal({ closeText: ł('%modal.close') });
        } */
    }
  };

  return Views;

})(this);

Chiffon.Views.Home = (function() {
  'use strict';

  var Views = Chiffon.Views;
  var View = Views.View;
  var Home = {};

  Home.About = (function() {
    function About(context) { View.call(this, context); }
    _.extend(About.prototype, Views.LayoutMixin);
    return About;
  })();

  Home.Contact = (function() {
    function Contact(context) { View.call(this, context); }
    _.extend(Contact.prototype, Views.LayoutMixin);
    return Contact;
  })();

  Home.Index = (function() {
    function Index(context) { View.call(this, context); }
    Index.prototype.initCore = function() {
      $('.vignette').watermark(ł('%preview.watermark'));
    };
    _.extend(Index.prototype, Views.LayoutMixin);
    return Index;
  })();

  return Home;

})();

Chiffon.Views.Account = (function() {
  'use strict';

  var Views = Chiffon.Views;
  var View = Views.View;
  var Account = {};

  Account.Newsletter = (function() {
    function Newsletter(context) { View.call(this, context); }
    _.extend(Newsletter.prototype, Views.LayoutMixin);
    return Newsletter;
  })();

  Account.Login = (function() {
    function Login(context) { View.call(this, context); }

    Login.prototype.initCore = function() {
      this.validate(function() {
        $('#login_form').validate({
          messages: { token: ł('%login.password_required') },
          rules: { token: { required: true, minlength: 10 } }
        });
      });
    };

    _.extend(Login.prototype, Views.LayoutMixin, Views.ValidateMixin);

    return Login;
  })();

  Account.Register = (function() {
    function Register(context) { View.call(this, context); }

    Register.prototype.initCore = function() {
      var $form = $('#register_form');

      this.validate(function() {
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
    };

    _.extend(Register.prototype, Views.LayoutMixin, Views.ValidateMixin);
    return Register;
  })();

  return Account;

})();

Chiffon.Views.Designer = (function(window) {
  'use strict';

  var location = window.location;
  var Components = Chiffon.Components;
  var Views = Chiffon.Views;
  var View = Views.View;
  var Designer = {};

  //var DesignerLayoutMixin = (function() {
  //  function DesignerLayout(context) {
  //    BaseView.apply(this, arguments);
  //    this.layoutView = new Views.Layout(context);
  //  }

  //  DesignerLayout.prototype = {
  //    init: function() {
  //      this.layoutView.init();
  //      Components.StickyInfo();
  //    }
  //  };

  //  return DesignerLayout;
  //})();

  var DesignerLayoutMixin;

  Designer.Index = (function() {
    function Index(context) { View.call(this, context); }
    _.extend(Index.prototype, DesignerLayoutMixin);
    return Index;
  })();

  Designer.Category = (function() {
    function Category(context) { View.call(this, context); }
    _.extend(Category.prototype, DesignerLayoutMixin);
    return Category;
  })();

  Designer.Pattern = (function() {
    function Pattern(context) { View.call(this, context); }

    Pattern.prototype.initCore = function() {
      // NB: location.hash contient le caractère '#'.
      Components.ViewNavigator({ currentSel: location.hash });
    };

    _.extend(Pattern.prototype, DesignerLayoutMixin);
    return Pattern;
  })();

  return Designer;

})(this);
