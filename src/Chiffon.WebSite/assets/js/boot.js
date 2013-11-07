/*jshint laxcomma: true, laxbreak:true*/
/*jslint nomen:true, white: true, todo: true*/
/*global window, yepnope*/

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

  function buildresources(debug, version) {
    // Manière naïve de détecter les nouveaux navigateurs.
    var modern = '__proto__' in {}
      , infix = (modern ? '-' : '.compat-') + version;

    function getbundle(name) {
      return name + infix + '.min.js';
    }

    if (debug) {
      return {
        chiffon: [
          modern ? 'vendor/jquery-2.0.3.min.js' : 'vendor/jquery-1.10.1.min.js'
          , modern ? 'vendor/lodash-2.2.1.min.js' : 'vendor/lodash.compat-2.2.1.min.js'
          , 'vendor/l10n-2013.09.12.js'
          // Plugins jQuery : jquery.watermark, jquery.external & jquery.modal.
          , 'jquery.plugins.js'
          , 'jquery.modal.js'
          // Internationalisation : librairie & ressources.
          , 'chiffon.localization.js'
          , 'chiffon.js'
        ]
      };
    } else {
      return {
        chiffon: [getbundle('chiffon')]
      };
    }
  }

  resources = buildresources(args.debug, args.version)

  // TODO: Désactiver la suite pour les petits écrans ?
  // Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html

  require(resources.chiffon, function() {
    var ChiffonClass = window.Chiffon, chiffon;
    if (undef === ChiffonClass) { return; }

    // Configuration de L10N.
    String.locale = args.locale;

    chiffon = new ChiffonClass({
      require: require
      , resources: resources
      , user: user = {
        isAnonymous: !args.isAuth
        , isAuth: args.isAuth
      }
    });

    chiffon.handle(args.controller, args.action);
  });
}(window.args));
