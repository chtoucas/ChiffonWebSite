;

this.App = (function(win, _, yepnope, undef) {
  'use strict';

  // Configuration par défaut.
  var defaults = {
    baseUrl: '/assets/js/'
    , debug: false
    , domain: 'pourquelmotifsimone.com'
    , gaq: 'UA-43374044-1'
    , version: undef
  };

  // Langues supportées.
  var locales = ['fr', 'en'];

  return function(options) {
    var settings = _.defaults(options || {}, defaults);

    if ('/' !== settings.baseUrl.substring(-1, 1)) {
      settings.baseUrl = settings.baseUrl + '/';
    }

    function rebase(src) { return settings.baseUrl + src; }

    function vendor(src) { return settings.baseUrl + 'vendor/' + src; }

    this.dependencies = {
      chiffon: function() {
        return settings.debug
          ? ['vendor/l10n-2013.04.18.min.js', 'localization.js', 'chiffon.js'].map(rebase)
          : rebase('chiffon-' + settings.version + '.min.js');
      }

      , googleAnalytics: '//www.google-analytics.com/ga.js'

      , jQuery: function() {
        return vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js');
      }

      //, jQueryCookie: function() { return vendor('jquery.cookie-1.3.1.min.js'); }

      //, jQueryOutside: function() { return vendor('jquery.ba-outside-events-1.1.min.js'); }

      , jQueryValidate: function(locale) {
        return ['jquery.validate.min.js', 'additional-methods.min.js']
          .push('localization/messages_' + locale + '.js')
          .map(function(src) { return vendor('jquery.validate-1.11.1/' + src); });
      }
    };

    this.main = function(locale, fn) {
      var that = this;

      if (-1 === locales.indexOf(locale)) {
        throw new Error('The locale "' + locale + '" is not supported.');
      }

      // Google Analytics.
      yepnope({
        test: settings.gaq
        , yep: this.dependencies.googleAnalytics
        , callback: function() {
          var ga = win.ga;
          if (undef === ga) { return; }

          ga('create', settings.gaq, settings.domain);
          ga('send', 'pageview');
        }
      });

      // FIXME: Quid quand un des appels échoue ?
      yepnope({
        load: [this.dependencies.jQuery()].concat(this.dependencies.chiffon())
        , complete: function() {
          var Chiffon = win.Chiffon;
          if (undef === Chiffon) { return; }

          var context = {
            locale: locale
            , app: that
          };

          fn(new Chiffon(context));
        }
      });
    };
  }

})(this, this._, this.yepnope);
