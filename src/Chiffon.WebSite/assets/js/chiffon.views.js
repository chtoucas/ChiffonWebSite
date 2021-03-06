﻿/*global  _, $, Chiffon*/

Chiffon.Utils = (function(window, undef) {
  'use strict';

  var location = window.location;
  var Utils = {};

  // WARNING: cette fonction ne prend pas en compte les paramètres multiples.
  Utils.parseQuery = function(value) {
    // NB: window.location.search commence avec le caractère '?'.
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
        // Si on a une seule vue, pas besoin d'aller plus loin.
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

  return Components;

})(this);
/*jshint +W074*/

Chiffon.Views = (function(window, undef) {
  'use strict';

  var View;
  var LayoutMixin;
  var validationResources;
  var Views = {};

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
    //initLayout: function() {
    //}
  };

  Views.ValidateMixin = {
    // Chargement de jQuery.validate puis exécution d'un callback.
    validate: function(fn) {
      if (undef === validationResources) {
        validationResources = [DEBUG ? 'jquery.validate.js' : 'jquery.validate.min.js']
          // NB: Pour le moment on n'affiche pas les messages d'erreur.
          .map(function(src) { return 'vendor/jquery.validate-1.13.1/' + src; });
      }

      this.context.require(validationResources, function() {
        $.validator.setDefaults({
          hightlight: function(elmt) { $(elmt).addClass('error'); },
          unhightlight: function(elmt) { $(elmt).removeClass('error'); }
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

  var Views = Chiffon.Views;
  var Simple = Chiffon.Views.Simple;

  return {
    About: Simple,

    Contact: Simple.Create(function() {
      var $form = $('#contact_form');

      this.validate(function() {
        $form.validate({
          // NB: On ne veut pas de message d'erreur par "input".
          // TODO: "errorPlacement" ne semble pas être la bonne méthode à utiliser.
          errorPlacement: $.noop,
          messages: {
            Email: '',
            Name: '',
            Message: ''
          },
          rules: {
            Email: { required: true, email: true },
            Name: { required: true, minlength: 2, maxlength: 200 },
            Message: { required: true, minlength: 10, maxlength: 3000 }
          }
        });
      });
    }, Views.ValidateMixin),

    Index: Simple
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
        // NB: On ne veut pas de message d'erreur par "input".
        // TODO: "errorPlacement" ne semble pas être la bonne méthode à utiliser.
        errorPlacement: $.noop,
        messages: {
          email: '',
          password: ''
        },
        rules: {
          email: { required: true, email: true },
          password: { required: true, minlength: 7 }
        }
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
          LastName: '',
          FirstName: '',
          CompanyName: '',
          Email: ''
        },
        rules: {
          LastName: { required: true, minlength: 2, maxlength: 50 },
          FirstName: { required: true, minlength: 2, maxlength: 50 },
          CompanyName: { required: true, minlength: 2, maxlength: 100 },
          Email: { required: true, email: true }
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

      this.context.require([DEBUG ? 'vendor/jquery.waypoints-3.1.1.js' : 'vendor/jquery.waypoints-3.1.1.min.js'], function() {
        // On cache la pagination.
        $pager.hide();

        $container.waypoint({
          offset: 'bottom-in-view',

          handler: function(direction) {
            if (direction !== 'down') {
              return;
            }

            var waypoint = this;
            var $more = $(moreSel);

            // On désactive "waypoint" le temps de la récupération du contenu de la page suivante.
            //$this.waypoint('disable');
            waypoint.disable();
            // On affiche un indicateur visuel de chargement.
            $loading.appendTo($container);

            // NB: On n'utilise pas la configuration global.
            return $.ajax({ url: $more.attr('href'), global: false })
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
                  waypoint.enable();
                } else {
                  // On est arrivé en bout de course, on peut supprimer complètement "waypoint".
                  waypoint.destroy();
                }
              })
              .fail(function() {
                $loading.remove();
                // NB: On réactive la pagination, mais uniquement à partir de la page qui n'a
                // pas pu être chargée.
                $pager.show();
                waypoint.destroy();
              });
          }
        });
      });
    }
  };

  var Designer = {
    Index: create({}, DesignerLayoutMixin),

    Category: create({}, DesignerLayoutMixin),

    // FIXME: Il n'est plus nécessaire d'utiliser DesignerLayoutMixin.
    Pattern: create({
      initCore: function() {
        // NB: location.hash contient le caractère '#'.
        Components.ViewNavigator({ currentSel: location.hash });
      }
    }, DesignerLayoutMixin)
  };

  return Designer;

})(this);
