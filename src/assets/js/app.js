;

this.App = (function(win, _, yepnope, undef) {
  'use strict';

  // Configuration par défaut.
  var defaults = {
    baseUrl: undef
    , debug: false
    , version: undef
  };

  // Langues supportées.
  var locales = ['fr', 'en'];

  return function(options) {
    var settings = _.defaults(options || {}, defaults);

    var dependencies = {
      chiffon: function() {
        return settings.debug
          ? ['vendor/l10n-2013.09.19.min.js', 'localization.js', 'chiffon.js'].map(rebase)
          : rebase('chiffon-' + settings.version + '.min.js');
      }

      , jQuery: function() {
        return vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js');
      }

      //, jQueryCookie: function() { return vendor('jquery.cookie-1.3.1.min.js'); }

      //, jQueryOutside: function() { return vendor('jquery.ba-outside-events-1.1.min.js'); }

      , jQueryValidate: function(locale) {
        var scripts = ['jquery.validate.min.js']
        if ('en' !== locale) { scripts.push('localization/messages_' + locale + '.js'); }
        return scripts.map(function(src) { return vendor('jquery.validate-1.11.1/' + src); });
      }
    };

    if (undef === settings.baseUrl) {
      throw new Error('The baseUrl is not defined.');
    } else if ('/' !== settings.baseUrl.substring(-1, 1)) {
      settings.baseUrl = settings.baseUrl + '/';
    }

    function rebase(src) { return settings.baseUrl + src; }

    function vendor(src) { return settings.baseUrl + 'vendor/' + src; }

    this.loadjQueryValidate = function(locale, onComplete) {
      yepnope({
        load: dependencies.jQueryValidate(locale)
        , complete: onComplete
      });
    };

    this.main = function(locale, fn) {
      var that = this;

      if (-1 === locales.indexOf(locale)) {
        throw new Error('The locale "' + locale + '" is not supported.');
      }

      // FIXME: Quid quand un des appels échoue ?
      yepnope({
        load: [dependencies.jQuery()].concat(dependencies.chiffon())
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
