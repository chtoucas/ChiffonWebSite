/*global window, args, yepnope*/

var App = (function(undef) {
  'use strict';

  var App = {};

  function buildResources(version, locale, debug) {
    var core_resources;
    var bundle_postfix = '-' + version + '.js';

    if (debug) {
      // NB: Ne pas utiliser de version minifiée en mode debug, même si on dispose du sourcemap.
      core_resources = [
        'vendor/jquery-2.0.3.js',
        'vendor/lodash-2.2.1.js',
        'vendor/l10n-2013.09.12.js',
        'jquery.modal.js',
        'chiffon.jquery.js',
        'chiffon.localization.js',
        'chiffon.core.js'
      ];
    } else {
      core_resources = ['lib', 'core'].map(function(name) { return '_' + name + bundle_postfix; });
    }

    return {
      core: core_resources,
      validation: ['jquery.validate.min.js']
        .concat('en' !== locale ? ['localization/messages_' + locale + '.js'] : [])
        .map(function(src) { return 'vendor/jquery.validate-1.11.1/' + src; })
    };
  }

  App.validateArgs = function(args) {
    // URL de base.
    if (undef === args.baseUrl) {
      throw new Error('The baseUrl is not defined.');
    }
    if ('/' !== args.baseUrl.substring(-1, 1)) {
      args.baseUrl = args.baseUrl + '/';
    }

    // Langues prises en charge.
    if (-1 === ['fr', 'en'].indexOf(args.locale)) {
      throw new Error('The locale "' + args.locale + '" is not supported.');
    }

    return args;
  };

  App.main = function(args) {
    var params = App.validateArgs(args);
    var resources = buildResources(params.version, params.locale, params.debug);
    var base_url = params.baseUrl;
    var require = function(resources, onComplete) {
      yepnope({
        load: resources.map(function(src) { return base_url + src; }),
        complete: onComplete
      });
    };

    // Configuration de L10N.
    String.locale = params.locale;

    require(resources.core, function() {
      // FIXME
      var ChiffonClass = window.Chiffon, context;
      if (undef === ChiffonClass) { return; }

      context = {
        require: require,
        resources: resources,
        isAnonymous: !params.isAuth
      };

      (new ChiffonClass(context)).handle(params.controller, params.action);
    });
  };

  return App;
})();

// On exécute automatiquement App.main si args est défini (utile pour pouvoir tester App).
// Pour les anciens navigateurs, on désactive complètement JavaScript.
// NB: La méthode de détection utilisée est assez naïve mais devrait faire l'affaire.
//  Cf. http://kangax.github.io/es5-compat-table/
// TODO: Désactiver aussi suite pour les petits écrans ?
//  Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html
if (undefined !== args && typeof 'Function.prototype.bind' === 'function') {
  App.main(args);
}
