/*global args, yepnope, _*/

var Chiffon = (function(window, undef) {
  'use strict';

  // Configuration par défaut.
  var defaultSettings = {
    ajaxTimeout: 3000
  };
  var defaultContext = {
    baseUrl: '//wznw.org/chiffon/',
    isAuth: false,
    locale: 'fr'
  };
  var baseUrls = [defaultContext.baseUrl, '/assets/js/'];
  var locales = [defaultContext.locale, 'en'];
  var bundlePostfix = '-' + VERSION + '.js';

  // Utilitaires de validation.

  function validateBaseUrl(args) {
    if (-1 === baseUrls.indexOf(args.baseUrl)) {
      if (DEBUG) { console.log('The baseUrl "' + args.baseUrl + '" is not valid.'); }
      args.baseUrl = defaultContext.baseUrl;
    }
    return args;
  }

  function validateIsAuth(args) {
    args.isAuth = true === args.isAuth;
    return args;
  }

  function validateLocale(args) {
    if (-1 === locales.indexOf(args.locale)) {
      if (DEBUG) { console.log('The locale "' + args.locale + '" is not supported.'); }
      args.locale = defaultContext.locale;
    }
    return args;
  }

  // Objet Chiffon.

  function Chiffon(context) {
    this.context = context;
  }

  Chiffon.getBundle = function (name) { return '_' + name + bundlePostfix; };

  // Configuration globale de l'application.
  // WARNING: On suppose au préalable que jQuery a été chargé.
  Chiffon.init = function(options) {
    var settings = _.defaults(options || {}, defaultSettings);

    // Comportement des appels Ajax via jQuery.
    window.$.ajaxSetup({
      timeout: settings.ajaxTimeout,
      async: true,
      cache: true
    });
  };

  // Configuration de la langue en cours d'utilisation.
  // WARNING: On définit ici une fonction dans le contexte global.
  Chiffon.initLocale = function(locale) {
    // Configuration de L10N.
    String.locale = locale;

    // Utilitaire de localisation d'une chaîne de caractères.
    window.ł = function(value) { return value.toLocaleString(); };
  };

  // Valide et retourne le contexte de la requête.
  Chiffon.validateContext = function(context) {
    return validateIsAuth(validateLocale(validateBaseUrl(context)));
  };

  // Principal point d'entrée de l'application.
  Chiffon.main = function(args) {
    var context = Chiffon.validateContext(_.defaults(args.context || {}, defaultContext));
    // Mise en cache de baseUrl.
    var baseUrl = context.baseUrl;
    var coreResources = DEBUG ? [
      // NB: Ne pas utiliser de version minifiée, même si on dispose du sourcemap.
      '__proto__' in {} ? 'vendor/zepto-1.0.0.js' : 'vendor/jquery-2.0.3.js',
      //'vendor/jquery-2.0.3.js',
      'vendor/l10n-2013.09.12.js',
      'jquery.modal.js',
      'chiffon.jquery.js',
      'chiffon.localization.js',
      'chiffon.core.js'
    ] : [
      '__proto__' in {} ? 'vendor/zepto-1.0.0.min.js' : 'vendor/jquery-2.0.3.min.js',
      //'vendor/jquery-2.0.3.min.js',
      Chiffon.getBundle('core')
    ];

    context.require = function(resources, onComplete) {
      yepnope({
        load: resources.map(function(src) { return baseUrl + src; }),
        complete: onComplete
      });
    };

    context.require(coreResources, function() {
      if (undef === window.$ || undef === Chiffon.Views) {
        if (DEBUG) { console.log('Could not load core dependencies.'); }
        return;
      }

      Chiffon.init(args.settings);

      (new Chiffon(context)).handle(args.request);
    });
  };

  Chiffon.prototype = {
    handle: function(request) {
      var req = _.defaults(request || {}, { action: '', controller: '', params: {} });

      Chiffon.initLocale(this.context.locale);

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
if (undefined !== args && typeof Function.prototype.bind === 'function') {
  Chiffon.main(args);
}
