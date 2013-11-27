/*global  _, $, ł, Chiffon*/

Chiffon.Utils = (function(window, undef) {
  'use strict';

  var location = window.location;
  var Utils = {};

  // WARNING: cette fonction ne prend pas en compte les paramètres multiples.
  Utils.parseQuery = function(value) {
    // WARNING: window.location.search commence avec le caractère '?'.
    if (undef === value) { value = location.search.substring(1); }

    if (!value) { return {}; }

    // TODO: /(?:^|&)([^&=]*)=?([^&]*)/g
    var parser = /([^&=]+)=?([^&]*)/g;
    var decode = function(s) {
      return decodeURIComponent(s.replace(/\+/g, ' '));
    };
    var pairs = {};

    value.replace(parser, function(match, k, v) {
      var key = decode(k);
      if (!key) { return; }
      pairs[key] = decode(v);
    });

    return pairs;
  };

  return Utils;
})(this);

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
      viewsSel: '#pattern LI',
      navSel: '#pattern_nav',
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

  //// Autant que possible on s'assure que le bloc informations sur le designer est toujours visible.
  //Components.StickyInfo = (function() {
  //  // Configuration par défaut.
  //  var settings;
  //  var defaults = {
  //    infoSel: '#info',
  //    designerSel: '#designer'
  //  };

  //  var $info;
  //  var $designer;

  //  // Géométrie du bloc #info.
  //  var infoHeight;
  //  var infoWidth;
  //  var infoTop;
  //  var infoLeft;

  //  // Géométrie du bloc #designer.
  //  var designerWidth;

  //  // Géométrie de la fenêtre.
  //  var windowHeight;

  //  function handleScrollEventForMediumBlock() {
  //    var scrollLimit;
  //    var stickyClass = 'info_sticky info_top';
  //    var isSticky = false;

  //    function stick() {
  //      if (isSticky) { return; }
  //      isSticky = true;
  //      $info.addClass(stickyClass);
  //    }

  //    function unstick() {
  //      if (!isSticky) { return; }
  //      isSticky = false;
  //      $info.removeClass(stickyClass);
  //    }

  //    scrollLimit = infoTop - 23;

  //    if ($(window).scrollTop() >= scrollLimit) { stick(); }

  //    $(window).scroll(function() {
  //      if ($(window).scrollTop() >= scrollLimit) { stick(); } else { unstick(); }
  //    });
  //  }

  //  // FIXME: Pour le moment, on ne s'occupe que des redimensionnements horizontaux.
  //  function handleResizeEvent() {
  //    // On utilise "_.debounce()" pour temporiser la prise en charge de l'évènement "resize"
  //    // pendant 150 millisecondes.
  //    $(window).resize(_.debounce(function() {
  //      var left = $designer.offset().left + designerWidth - infoWidth;

  //      $info.css({ 'left': left + 'px' });
  //    }, 150));
  //  }

  //  // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
  //  // pour qu'il soit toujours visible on lui donne une position fixe.
  //  function setupSmallBlock() {
  //    $info.addClass('info_sticky');
  //    $info.css({ top: infoTop + 'px', left: infoLeft + 'px' });
  //  }

  //  // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
  //  // haut de la fenêtre.
  //  function setupMediumBlock() {
  //    // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
  //    // plus tard lors d'un redimensionnement horizontal de la fenêtre.
  //    $info.css('left', infoLeft + 'px');

  //    handleScrollEventForMediumBlock();
  //  }

  //  function init() {
  //    var infoOffset;

  //    $info = $(settings.infoSel);
  //    $designer = $(settings.designerSel);

  //    infoHeight = $info.height();
  //    infoWidth = $info.width();

  //    infoOffset = $info.offset();
  //    infoTop = infoOffset.top;
  //    infoLeft = infoOffset.left;

  //    designerWidth = $designer.width();
  //    windowHeight = $(window).height();
  //  }

  //  return function(options) {
  //    settings = _.defaults(options || {}, defaults);

  //    init();

  //    if (windowHeight >= infoHeight + infoTop) {
  //      setupSmallBlock();
  //    } else if (windowHeight >= infoHeight) {
  //      setupMediumBlock();
  //    } else {
  //      // La fenêtre est trop petite pour contenir tout le bloc info.
  //      // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
  //      // On ne touche donc à rien.
  //      return;
  //    }

  //    handleResizeEvent();
  //  };
  //})();

  return Components;

})(this);
/*jshint +W074*/

Chiffon.Views = (function(window, undef) {
  'use strict';

  var View;
  var LayoutMixin;
  var validationResources;
  var document = window.document;
  var Views = {};

  function initModal() {
    // NB: On place l'événement sur "document" car on veut rester dans la modale après un clic.
    $(document).on('click.modal', 'A[rel="modal:open"]', function(e) {
      e.preventDefault();

      $(this).modal({ closeText: ł('%modal.close') });
    });
  }

  Views.View = View = function(context) {
    this.context = context;
  };

  View.prototype = {
    init: function() { this.initLayout(); this.initCore(); },
    initLayout: $.noop,
    initCore: $.noop
  };

  Views.Create = function(proto /*, mixins */) {
    var mixins = _.rest(arguments, 1);
    var view = function(context) { View.call(this, context); };
    var args = [view.prototype, proto].concat(mixins).concat(View.prototype);
    _.defaults.apply(undef, args);
    return view;
  };

  Views.LayoutMixin = LayoutMixin = {
    initLayout: function() {
      if (!this.context.isAuth) {
        // Pour les visiteurs anonymes, on active les modales.
        initModal();
      }
      //else {
      // // TODO: Utiliser un hashcode pour afficher la confirmation de compte.
      // $('<div class="welcome serif serif_large"><h2>Bienvenue !</h2><p>Merci de vous être inscrit.</p></div>')
      //   .appendTo('body').modal({ closeText: ł('%modal.close') });
      //}
    }
  };

  Views.ValidateMixin = {
    // Chargement de jQuery.validate puis exécution d'un callback.
    validate: function(fn) {
      var locale = this.context.locale;
      if (undef === validationResources) {
        validationResources = [DEBUG ? 'jquery.validate.js' : 'jquery.validate.min.js']
          .concat('en' !== locale ? ['localization/messages_' + locale + '.js'] : [])
          .map(function(src) { return 'vendor/jquery.validate-1.11.1/' + src; });
      }

      this.context.require(validationResources, function() {
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

  Views.Simple = Views.Create({}, LayoutMixin);

  Views.Simple.Create = function(fn) {
    var mixins = _.rest(arguments, 1);
    return Views.Create.apply(undef, [{ initCore: fn }, LayoutMixin].concat(mixins));
  };

  return Views;

})(this);

Chiffon.Views.Home = (function() {
  'use strict';

  var Simple = Chiffon.Views.Simple;

  return {
    About: Simple,
    Contact: Simple.Create(function() {
      // On ouvre les liens externes dans une nouvelle fenêtre.
      $('A[rel=external]').external();
    }),

    Index: Simple.Create(function() {
      $('.vignette').watermark(ł('%preview.watermark'));
    })
  };

})();

Chiffon.Views.Account = (function() {
  'use strict';

  var Views = Chiffon.Views;
  var Simple = Views.Simple;
  var ValidateMixin = Views.ValidateMixin;
  var Account = {
    Newsletter: Simple
  };

  Account.Login = Simple.Create(function() {
    var $form = $('#login_form');

    this.validate(function() {
      $form.validate({
        messages: { token: ł('%login.password_required') },
        rules: { token: { required: true, minlength: 10 } }
      });
    });
  }, ValidateMixin);

  Account.Register = Simple.Create(function() {
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
  }, ValidateMixin);

  return Account;

})();

Chiffon.Views.Designer = (function(window, undef) {
  'use strict';

  var $pager;
  var location = window.location;
  var Components = Chiffon.Components;
  var Utils = Chiffon.Utils;
  var Views = Chiffon.Views;
  var create = Views.Create;

  var DesignerLayoutMixin = {
    initLayout: function() {
      // FIXME: Rétablir cette fonctionnalité quand on aura fixé tous les bugs :-)
      //Components.StickyInfo();

      // Toutes les pages "designer" contiennent une liste de motifs et on essaie
      // de rendre la pagination plus ergonomique. Ainsi, on charge automatiquement 
      // le contenu de la page suivante et on affiche le résultat en bas de page.
      // NB: Si on clique dans un motif on garde la pagination d'origine pour ne pas perdre
      // le visiteur. On ne doit donc pas redescendre tout en base de la page pour retrouver
      // là où on en était.
      if (this.canScrollInfinitely()) {
        this.scrollInfinitely();
      }
    },

    // On peut utiliser la descente infinie quand il y a pagination et qu'on est à la première page.
    canScrollInfinitely: function() {
      var page;

      // On vérifie qu'il y a un pager.
      $pager = $('#pager');

      if (1 !== $pager.length) {
        return false;
      }

      // TODO: Récupérer la page de "this.context.params.p".
      page = Utils.parseQuery().p;

      // On n'active pas la pagination infinie que si on en est à la première page.
      return undef === page || '1' === page;
    },

    // Descente infinie.  
    scrollInfinitely: function() {
      var moreSel = '#next_page';
      var itemsSel = '#patterns LI';
      var $container = $('#patterns');
      var $loading = $('<li class=loading></li>');

      this.context.require([DEBUG ? 'vendor/jquery.waypoints-2.0.3.js' : 'vendor/jquery.waypoints-2.0.3.min.js'], function() {
        // On cache la pagination.
        $pager.hide();

        $container.waypoint({
          offset: 'bottom-in-view',

          handler: function(direction) {
            if (direction !== 'down') {
              return;
            }

            var $this = $(this);
            var $more = $(moreSel);

            // On désactive "waypoint" le temps de la récupération du contenu de la page suivante.
            $this.waypoint('disable');
            // On affiche un indicateur visuel de chargement.
            $loading.appendTo($container);

            return $.get($more.attr('href'))
              .done(function(data) {
                var $data = $($.parseHTML(data));
                var $newMore = $data.find(moreSel);

                // On cache l'indicateur visuel de chargement.
                $loading.remove();
                // On ajoute le contenu de la page suivante.
                $container.append($data.find(itemsSel));

                if ($newMore.length > 0) {
                  // On met à jour le lien "page suivante".
                  $more.replaceWith($newMore);
                  // On active à nouveau "waypoint".
                  $this.waypoint('enable');
                } else {
                  // On est arrivé en bout de course, on peut supprimer complètement "waypoint".
                  $this.waypoint('destroy');
                }
              })
              .fail(function() {
                $loading.remove();
                // NB: On réactive la pagination, mais uniquement à partir de la page qui n'a
                // pas pu être chargée.
                $pager.show();
                $this.waypoint('destroy');
              });
          }
        });
      });
    }
  };

  var Designer = {
    Index: create({}, DesignerLayoutMixin),

    Category: create({}, DesignerLayoutMixin),

    Pattern: create({
      initCore: function() {
        // NB: location.hash contient le caractère '#'.
        Components.ViewNavigator({ currentSel: location.hash });
      }
    }, DesignerLayoutMixin)
  };

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

  return Designer;

})(this);
