/*global  _, FastClick, yepnope*/

var Chiffon = (function(window, undef) {
  'use strict';

  // Configuration par défaut.
  var defaultSettings = {
    ajaxTimeout: 3000
  };
  var defaultContext = {
    baseUrl: '//wznw.org/chiffon/js/',
    //device: {
    //  isTouch: false
    //},
    isAuth: false,
    locale: 'fr'
  };
  var baseUrls = [defaultContext.baseUrl, '/assets/js/'];
  var locales = [defaultContext.locale, 'en'];
  var bundleSuffix = '-' + VERSION + '.js';
  var document = window.document;

  // Objet Chiffon.

  function Chiffon(context) {
    this.context = context;
  }

  Chiffon.getBundle = function(name) { return '_' + name + bundleSuffix; };

  // Configuration globale de l'application.
  // WARNING: On ajoute la fonction "ł" au contexte global.
  Chiffon.configure = function(options) {
    var settings = _.defaults(options || {}, defaultSettings);
    var $ = window.$;
    var nprogress = window.NProgress;

    // Utilitaire de localisation d'une chaîne de caractères.
    window.ł = function(value) { return value.toLocaleString(); };

    // Pour les tablettes, on essaie d'éliminer le temps de latence entre l'événement "touch"
    // et l'événement "click".
    $(function() {
      FastClick.attach(document.body);
    });

    // Comportement des appels Ajax via jQuery.
    $.ajaxSetup({
      timeout: settings.ajaxTimeout,
      async: true,
      cache: true
    });

    // Quand une requête ajax démarre on affiche un indicateur, idem quand un batch de requêtes se termine.
    $(document).ajaxStart(function() {
      nprogress.start();
    }).ajaxStop(function() {
      nprogress.done();
    });
  };

  // Valide puis retourne le contexte de la requête.
  Chiffon.validateContext = function(context) {
    // URL de base.
    if (-1 === baseUrls.indexOf(context.baseUrl)) {
      if (DEBUG) { console.log('The baseUrl "' + context.baseUrl + '" is not valid.'); }
      context.baseUrl = defaultContext.baseUrl;
    }
    // Authentifié ?
    context.isAuth = true === context.isAuth;
    // Langue demandée.
    if (-1 === locales.indexOf(context.locale)) {
      if (DEBUG) { console.log('The locale "' + context.locale + '" is not supported.'); }
      context.locale = defaultContext.locale;
    }
    return context;
  };

  // Principal point d'entrée de l'application.
  Chiffon.main = function(args) {
    var context = Chiffon.validateContext(_.defaults(args.context || {}, defaultContext));
    // Mise en cache de baseUrl.
    var baseUrl = context.baseUrl;
    var coreResources = DEBUG ? [
      // NB: Ne pas utiliser de version minifiée, même si on dispose du sourcemap.
      'vendor/jquery-2.0.3.js',
      'vendor/l10n-2013.09.12.js',
      'vendor/nprogress-0.1.2.js',
      'vendor/jquery.microdata/jquery.microdata.js',
      'vendor/jquery.microdata/schemas.js',
      'jquery.modal.js',
      'chiffon.jquery.js',
      'chiffon.localization.js',
      'chiffon.views.js'
    ] : [
      'vendor/jquery-2.0.3.min.js',
      Chiffon.getBundle('views')
    ];

    // NB: Il n'est pas possible de détecter une tablette mais on peut s'approcher du résultat.
    // Cf. https://github.com/Modernizr/Modernizr/blob/master/feature-detects/touchevents.js
    // et http://stackoverflow.com/questions/4817029/whats-the-best-way-to-detect-a-touch-screen-device-using-javascript
    //context.device.isTouch = 'ontouchstart' in window || window.navigator.msMaxTouchPoints > 0;

    context.require = function(resources, onComplete) {
      yepnope({
        load: resources.map(function(src) { return baseUrl + src; }),
        complete: onComplete
      });
    };

    context.require(coreResources, function() {
      var $ = window.$;

      if (undef === $ || undef === Chiffon.Views) {
        if (DEBUG) { console.log('Could not load resources.'); }
        return;
      }

      Chiffon.configure(args.settings);

      (new Chiffon(context)).init().handle(args.request);
    });
  };

  Chiffon.prototype = {
    handle: function(request) {
      var req = _.defaults(request || {}, { action: '', controller: '', params: {} });

      this.handleCore(req.controller, req.action, req.params);
    },

    handleCore: function(controllerName, actionName, params) {
      var Views = Chiffon.Views;

      // On cherche l'objet Views.{controllerName}.
      if (!Views.hasOwnProperty(controllerName)) { return; }

      var ViewController = Views[controllerName];
      var ViewClass;

      // On cherche l'objet Views.{controllerName}.{actionName}.
      if (!ViewController.hasOwnProperty(actionName)) { return; }

      ViewClass = ViewController[actionName];
      (new ViewClass(this.context)).init(params);
    },

    init: function() {
      // Configuration de L10N.
      String.locale = this.context.locale;

      return this;
    }
  };

  return Chiffon;

})(this);

// On n'exécute automatiquement Chiffon.main que si args est défini. Cette restriction est utile
// car elle permet d'utiliser aussi ce fichier comme une librairie (par ex pour les tests).
// Pour les anciens navigateurs, on désactive complètement JavaScript.
// La méthode de détection utilisée est assez naïve mais devrait faire l'affaire.
//  Cf. http://kangax.github.io/es5-compat-table/
// TODO: Désactiver aussi pour les petits écrans ?
//  Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html
if (undefined !== this.args && typeof Function.prototype.bind === 'function') {
  Chiffon.main(this.args);
}
