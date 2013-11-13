/*global args, yepnope, _*/

var Chiffon = (function(window, undef) {
  'use strict';

  var coreResources;
  var bundlePostfix = '-' + VERSION + '.js';
  var defaultContext = {
    baseUrl: '//wznw.org/chiffon/',
    isAuth: false,
    locale: 'fr',
    resources: { }
  };
  var baseUrls = [defaultContext.baseUrl, '/assets/js/'];
  var locales = [defaultContext.locale, 'en'];

  // WARNING: La fusion ne se fait pas de manière récursive.
  function merge(obj, defaults) {
    var result = defaults;
    if (undef === obj) {
      return result;
    }
    for (var prop in obj) {
      if (obj.hasOwnProperty(prop)) {
        result[prop] = obj[prop];
      }
    }
    return result;
  }

  function getBundle(name) { return '_' + name + bundlePostfix; }

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

  function getValidationResources(locale) {
    return ['jquery.validate.min.js']
      .concat('en' !== locale ? ['localization/messages_' + locale + '.js'] : [])
      .map(function(src) { return 'vendor/jquery.validate-1.11.1/' + src; });
  }

  if (DEBUG) {
      // NB: Ne pas utiliser de version minifiée, même si on dispose du sourcemap.
    coreResources = [
      'vendor/jquery-2.0.3.js',
      'vendor/l10n-2013.09.12.js',
      'jquery.modal.js',
      'chiffon.jquery.js',
      'chiffon.localization.js',
      'chiffon.core.js'
    ];
  } else {
    coreResources = ['vendor/jquery-2.0.3.min.js', getBundle('core')];
      //.concat(['core'].map(getBundle));
  }

  function Chiffon(context) {
    this.context = context;
  }

  Chiffon.prototype = {
    handle: function(request) {
      var req = _.defaults(request || {}, { action: '', controller: '', params: {} });

      this.initLocale();

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

    initLocale: function() {
      var locale = this.context.locale;

      // Configuration de L10N.
      String.locale = locale;

      this.context.resources.validation = getValidationResources(locale);
    }
  };

  Chiffon.validateContext = function(context) {
    return validateIsAuth(validateLocale(validateBaseUrl(context)));
  };

  Chiffon.main = function(args) {
    var context = Chiffon.validateContext(merge(args.context, defaultContext));
    // Mise en cache de baseUrl.
    var baseUrl = context.baseUrl;

    context.require = function(resources, onComplete) {
      yepnope({
        load: resources.map(function(src) { return baseUrl + src; }),
        complete: onComplete
      });
    };

    context.require(coreResources, function() {
      if (undef === Chiffon.prototype.handle) {
        if (DEBUG) { console.log('Could not load "coreResources".'); }
        return;
      }

      Chiffon.init(args.settings);

      (new Chiffon(context)).handle(args.request);
    });
  };

  return Chiffon;
})(this);

// On n'exécute automatiquement Chiffon.main que si args est défini. Cette restriction est utile
// car elle permet d'inclure ce fichier en tant que librairie (par ex pour les tests).
// Pour les anciens navigateurs, on désactive complètement JavaScript.
// La méthode de détection utilisée est assez naïve mais devrait faire l'affaire.
//  Cf. http://kangax.github.io/es5-compat-table/
// TODO: Désactiver aussi pour les petits écrans ?
//  Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html
if (undefined !== args && typeof Function.prototype.bind === 'function') {
  Chiffon.main(args);
}
