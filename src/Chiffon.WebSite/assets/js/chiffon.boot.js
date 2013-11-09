/*global yepnope*/

(function(args, undef) {
  'use strict';

  // Pour les anciens navigateurs, on désactive entièrement JavaScript.
  // La méthode de détection utilisée est assez naïve mais devrait faire l'affaire.
  // Cf. http://kangax.github.io/es5-compat-table/
  if (typeof 'Function.prototype.bind' !== 'function') { return; }

  var
  resources
  , core_resources
  , base_url = args.baseUrl
  , locale = args.locale
  , bundle_postfix = '-' + args.version + '.js'
  , require = function(resources, onComplete) {
    yepnope({
      load: resources.map(function(src) { return base_url + src; })
      , complete: onComplete
    });
  };

  // Validation des paramètres.
  if (undef === base_url) {
    throw new Error('The baseUrl is not defined.');
  }
  if ('/' !== base_url.substring(-1, 1)) {
    base_url = base_url + '/';
  }
  // Langues prises en charge.
  if (-1 === ['fr', 'en'].indexOf(locale)) {
    throw new Error('The locale "' + locale + '" is not supported.');
  }

  if (args.debug) {
    // NB: Ne pas utiliser de version minifiée en mode debug, même si on dispose du sourcemap.
    core_resources = [
      'vendor/jquery-2.0.3.js'
      , 'vendor/lodash-2.2.1.js'
      , 'vendor/l10n-2013.09.12.js'
      , 'jquery.plugins.js'
      , 'jquery.modal.js'
      , 'chiffon.localization.js'
      , 'chiffon.js'
    ];
  } else {
    core_resources = ['lib', 'core'].map(function(name) { return '_' + name + bundle_postfix; });
  }

  resources = {
    core: core_resources
    , jQueryValidate: ['jquery.validate.min.js']
      .concat('en' !== locale ? ['localization/messages_' + locale + '.js'] : [])
      .map(function(src) { return 'vendor/jquery.validate-1.11.1/' + src; })
  };

  // TODO: Désactiver la suite pour les petits écrans ?
  // Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html

  require(resources.core, function(undef) {
    var ChiffonClass = window.Chiffon, chiffon;
    if (undef === ChiffonClass) { return; }

    // Configuration de L10N.
    String.locale = locale;

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
