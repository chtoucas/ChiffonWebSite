/*global  _, $, FastClick, NProgress, yepnope*/

(function(window) {
  'use strict';

  // Configuration par défaut.
  var defaultSettings = {
    ajaxTimeout: 3000
  };

  // Contexte par défaut.
  var defaultContext = {
    baseUrl: '//wznw.org/chiffon/js/'
  };

  var document = window.document;

  /*
   * Configuration globale de l'application.
   */
  function configure(options) {
    var settings = _.defaults(options || {}, defaultSettings);

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
      NProgress.start();
    }).ajaxStop(function() {
      NProgress.done();
    });
  }

  /*
   * Objet Chiffon.
   */
  function Chiffon(context) {
    this.context = context;
  }

  /*
   * Point d'entrée de l'application.
   */
  Chiffon.main = function(args) {
    var context = _.defaults(args.context || {}, defaultContext);

    // Mise en cache de baseUrl.
    var baseUrl = context.baseUrl;

    context.require = function(resources, onComplete) {
      yepnope({
        load: resources.map(function(src) { return baseUrl + src; }),
        complete: onComplete
      });
    };

    configure(args.settings);

    (new Chiffon(context)).handle(args.request);
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

  window.Chiffon = Chiffon;
})(this);