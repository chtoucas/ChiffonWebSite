/*global  _, FastClick, yepnope*/

var Chiffon = (function(window, undef) {
  'use strict';

  // Configuration par défaut.
  var defaultSettings = {
    ajaxTimeout: 3000
  };

  // Contexte par défaut.
  var defaultContext = {
    baseUrl: '//wznw.org/chiffon/js/',
    isAuth: false
  };

  var bundleSuffix = '-' + VERSION + '.js';
  var document = window.document;

  /*
   * Objet Chiffon.
   */
  function Chiffon(context) {
    this.context = context;
  }

  Chiffon.getBundle = function(name) { return '_' + name + bundleSuffix; };

  /*
   * Configuration globale de l'application.
   * WARNING: On ajoute la fonction "ł" au contexte global.
   */
  Chiffon.configure = function(options) {
    var settings = _.defaults(options || {}, defaultSettings);
    var $ = window.$;
    var nprogress = window.NProgress;

    // Pour les tablettes, on essaie d'éliminer le temps de latence entre l'événement "touch"
    // et l'événement "click".
    $(function() {
      FastClick.attach(document.body);
    });

    // Définition du comportement des appels Ajax via jQuery.
    $.ajaxSetup({
      timeout: settings.ajaxTimeout,
      async: true,
      cache: true
    });

    // Quand une requête ajax démarre on affiche un indicateur, idem quand un batch
    // de requêtes se termine.
    $(document).ajaxStart(function() {
      nprogress.start();
    }).ajaxStop(function() {
      nprogress.done();
    });
  };

  /*
   * Valide puis retourne le contexte de la requête.
   */
  Chiffon.validateContext = function(context) {
    // Authentifié ?
    context.isAuth = true === context.isAuth;

    return context;
  };

  /*
   * Point d'entrée de l'application.
   */
  Chiffon.main = function(args) {
    var context = Chiffon.validateContext(_.defaults(args.context || {}, defaultContext));

    // Mise en cache de baseUrl.
    var baseUrl = context.baseUrl;

    var coreResources = DEBUG ? [
      // NB: Ne pas utiliser de version minifiée, même si on dispose du sourcemap.
      'vendor/jquery-2.1.1.js',
      'vendor/nprogress-0.1.6.js',
      'vendor/jquery.microdata/jquery.microdata.js',
      'vendor/jquery.microdata/schemas.js',
      'chiffon.jquery.js',
      'chiffon.views.js'
    ] : [
      'vendor/jquery-2.1.1.min.js',
      Chiffon.getBundle('views')
    ];

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

      (new Chiffon(context)).handle(args.request);
    });
  };

  /*
   * Prototype pour Chiffon.
   */
  Chiffon.prototype = {
    handle: function(request) {
      var req = _.defaults(request || {}, { action: '', controller: '' });

      this.handleCore(req.controller, req.action);
    },

    handleCore: function(controllerName, actionName) {
      var Views = Chiffon.Views;

      // On cherche l'objet Views.{controllerName}.
      if (!Views.hasOwnProperty(controllerName)) { return; }

      var ViewController = Views[controllerName];
      var ViewClass;

      // On cherche l'objet Views.{controllerName}.{actionName}.
      if (!ViewController.hasOwnProperty(actionName)) { return; }

      ViewClass = ViewController[actionName];
      (new ViewClass(this.context)).init();
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
