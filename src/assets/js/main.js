;

this.App = (function(win, yepnope, undef) {
  'use strict';

  var debug = false
    , version = '1.0'
    , locales = ['fr', 'en']
    , gaq = 'UA-43374044-1';

  return function() {
    var baseUrl = '/assets/js/';

    function rebase(src) { return baseUrl + src; }

    function vendor(src) { return baseUrl + 'vendor/' + src; }

    this.baseUrl = function(value) {
      if (undef === value) {
        throw new ReferenceError('The "baseUrl" value is undefined.');
      } else if ('/' !== value.substring(-1, 1)) {
        baseUrl = value + '/';
      } else {
        baseUrl = value;
      }
    };

    this.gaq = function(value) {
      if (undef === value) {
        throw new ReferenceError('The "gaq" value is undefined.');
      }
      gaq = value;
    };

    this.version = function(value) {
      if (undef === value) {
        throw new ReferenceError('The "version" value is undefined.');
      }
      version = value;
    };

    this.jQuery = function() {
      return vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js');
    };

    //this.jQueryCookie = function() { return vendor('jquery.cookie-1.3.1.min.js'); }

    //this.jQueryOutside = function() { return vendor('jquery.ba-outside-events-1.1.min.js'); }

    this.jQueryValidate = function(locale) {
      return ['jquery.validate.min.js', 'additional-methods.min.js']
        .push('localization/messages_' + locale + '.js')
        .map(function(src) { return vendor('jquery.validate-1.11.1/' + src); });
    };

    this.Chiffon = function() {
      return debug
        ? ['vendor/l10n-2013.04.18.min.js', 'localization.js', 'chiffon.js'].map(rebase)
        : rebase('chiffon-' + version + '.min.js');
    };

    this.main = function(locale, fn) {
      var that = this;

      if (-1 === locales.indexOf(locale)) {
        throw new Error('The locale "' + locale + '" is not supported.');
      }

      // FIXME: Quid quand un des appels échoue ?
      yepnope([
        {
          // Google Analytics.
          test: gaq
          , yep: '//www.google-analytics.com/ga.js'
          , callback: function() {
            ga('create', gaq, 'pourquelmotifsimone.com');
            ga('send', 'pageview');
          }
        }, {
          load: [Bundles.jQuery()].concat(Bundles.Chiffon())
          , complete: function() {
            var Chiffon = win.Chiffon;
            if (undef === Chiffon) { return; }

            var context = {
              locale: locale
              , app: that
            };

            fn(new Chiffon(context));
          }
        }
      ]);
    };
  }

})(this, this.yepnope);
