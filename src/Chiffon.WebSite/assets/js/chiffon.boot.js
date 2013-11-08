/*global yepnope*/

(function(args) {
  'use strict';

  var
  resources
  , require = function(resources, onComplete) {
    yepnope({
      load: resources.map(function(src) { return args.baseUrl + src; })
      , complete: onComplete
    });
  };

  function buildresources(debug, version, locale) {
    // Manière naïve de détecter les nouveaux navigateurs.
    var core_resources
      , modern = typeof 'Function.prototype.bind' === 'function'
      , compat_infix = (modern ? '' : '.compat')
      , bundle_postfix = '-' + version + '.js';

    if (debug) {
      // NB: Ne pas utiliser de version minifiée en mode debug.
      core_resources = (
        modern
        ? ['vendor/jquery-2.0.3.js', 'vendor/lodash-2.2.1.js']
        : ['vendor/jquery-1.10.2.js', 'vendor/lodash.compat-2.2.1.js']
      ).concat([
        'vendor/l10n-2013.09.12.js'
        // Plugins jQuery : jquery.watermark & jquery.external.
        , 'jquery.plugins.js'
        , 'jquery.modal.js'
        , 'chiffon.localization.js'
        , 'chiffon.js'
      ]);
    } else {
      core_resources = ['lib' + compat_infix, 'core'].map(function(name) {
        return '_' + name + bundle_postfix;
      });
    }

    return {
      core: core_resources
      , jQueryValidate: (function() {
        var scripts = ['jquery.validate.min.js'];
        if ('en' !== locale) { scripts.push('localization/messages_' + locale + '.js'); }
        return scripts.map(function(src) { return 'vendor/jquery.validate-1.11.1/' + src; });
      }())
    };
  }

  resources = buildresources(args.debug, args.version, args.locale);

  // TODO: Désactiver la suite pour les petits écrans ?
  // Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html

  require(resources.core, function(undef) {
    var ChiffonClass = window.Chiffon, chiffon;
    if (undef === ChiffonClass) { return; }

    // Configuration de L10N.
    String.locale = args.locale;

    chiffon = new ChiffonClass({
      require: require
      , resources: resources
      , user: {
        isAnonymous: !args.isAuth
        , isAuth: args.isAuth
      }
    });

    chiffon.handle(args.controller, args.action);
  });
}(window.args));
