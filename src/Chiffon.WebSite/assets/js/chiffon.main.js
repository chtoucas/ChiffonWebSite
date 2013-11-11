/*global args, yepnope*/

var Chiffon = (function(window, undef) {
  'use strict';

  var Chiffon;
  var getResources;
  var defaults = {
    action: '',
    controller: '',
    baseUrl: '//wznw.org/chiffon/',
    isAuth: false,
    locale: 'fr',
    params: {},
    settings: {}
  };
  var baseUrls = [defaults.baseUrl, '/'];
  var locales = [defaults.locale, 'en'];

  // NB: On ne fait pas de fusion récursive.
  function mergeDefaults(args) {
    var result = defaults;
    for (var prop in args) {
      if (args.hasOwnProperty(prop)) {
        result[prop] = args[prop];
      }
    }
    return result;
  }

  function validateBaseUrl(args) {
    if (-1 === baseUrls.indexOf(args.baseUrl)) {
      if (DEBUG) { console.log('The baseUrl "' + args.baseUrl + '" is not valid.'); }
      args.baseUrl = defaults.baseUrl;
    }
    return args;
  }

  function validateLocale(args) {
    if (-1 === locales.indexOf(args.locale)) {
      if (DEBUG) { console.log('The locale "' + args.locale + '" is not supported.'); }
      args.locale = defaults.locale;
    }
    return args;
  }

  function validateIsAuth(args) {
    args.isAuth = true === args.isAuth;
    return args;
  }

  function getValidationResources(locale) {
    return ['jquery.validate.min.js']
      .concat('en' !== locale ? ['localization/messages_' + locale + '.js'] : [])
      .map(function(src) { return 'vendor/jquery.validate-1.11.1/' + src; });
  }

  Chiffon.validateArgs = function(args) {
    // NB: controller et action seront validés dans Chiffon.handleCore.
    return validateIsAuth(validateLocale(validateBaseUrl(mergeDefaults(args))));
  };

  if (DEBUG) {
    getResources = function(locale) {
      // NB: Ne pas utiliser de version minifiée, même si on dispose du sourcemap.
      return {
        core: [
          'vendor/jquery-2.0.3.js',
          'vendor/lodash-2.2.1.js',
          'vendor/l10n-2013.09.12.js',
          'jquery.modal.js',
          'chiffon.jquery.js',
          'chiffon.localization.js',
          'chiffon.core.js'
        ],
        validation: getValidationResources(locale)
      };
    };
  } else {
    getResources = function(locale) {
      var bundlePostfix = '-' + VERSION + '.js';

      return {
        core: ['lib', 'core'].map(function(name) { return '_' + name + bundlePostfix; }),
        validation: getValidationResources(locale)
      };
    };
  }

  Chiffon = function(context) {
    this.context = context;
  };

  Chiffon.main = function(args) {
    var opts = Chiffon.validateArgs(args);
    var resources = getResources(opts.locale);
    // Mise en cache de la valeur de baseUrl.
    var baseUrl = opts.baseUrl;
    var require = function(resources, onComplete) {
      yepnope({
        load: resources.map(function(src) { return baseUrl + src; }),
        complete: onComplete
      });
    };

    require(resources.core, function() {
      if (undef === Chiffon.prototype.handle) {
        if (DEBUG) { console.log('Could not load resources.core.'); }
        return;
      }

      (new Chiffon({ isAuth: opts.isAuth, locale: opts.locale, require: require, resources: resources }))
        .init(opts.settings)
        .handle(opts.controller, opts.action, opts.params);
    });
  };

  return Chiffon;
})(this);

// On n'exécute automatiquement Chiffon.main que si args est défini. Cette restriction est utile
// car elle permet d'inclure ce fichier en tant que librairie (par ex pour les tests).
// Pour les anciens navigateurs, on désactive complètement JavaScript.
// NB: La méthode de détection utilisée est assez naïve mais devrait faire l'affaire.
//  Cf. http://kangax.github.io/es5-compat-table/
// TODO: Désactiver aussi suite pour les petits écrans ?
//  Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html
if (undefined !== args && typeof Function.prototype.bind === 'function') {
  Chiffon.main(args);
}
