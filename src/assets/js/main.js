;

this.Bundles = (function(undef) {
  'use strict';

  var Bundles = {
    baseUrl: '/assets/js/'

    , version: undef

    , rebase: function(src) { return Bundles.baseUrl + src; }

    , vendor: function(src) { return Bundles.baseUrl + 'vendor/' + src; }

    , jQuery: function() {
      return Bundles.vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js');
    }

    //, jQueryCookie: function() { return Bundles.vendor('jquery.cookie-1.3.1.min.js'); }

    //, jQueryOutside: function() { return Bundles.vendor('jquery.ba-outside-events-1.1.min.js'); }

    , jQueryValidate: function(locale) {
      return ['jquery.validate.min.js', 'additional-methods.min.js']
        .push('localization/messages_' + locale + '.js')
        .map(function(src) { return Bundles.vendor('jquery.validate-1.11.1/' + src); });
    }

    , Chiffon: function() {
      return undef !== Bundles.version
        ? rebase('chiffon-' + Bundles.version + '.min.js')
        : ['vendor/l10n-2013.04.18.min.js', 'localization.js', 'chiffon.js'].map(Bundles.rebase);
    }
  };

  return Bundles;
})();

this.main = (function(win, Dependencies, yepnope, undef) {
  'use strict';

  var locales = ['fr', 'en'];

  return function(locale, fn) {
    if (-1 === locales.indexOf(locale)) {
      throw new Error('The locale "' + locale + '" is not supported.');
    }

    // FIXME: Quid quand un des appels échoue ?
    yepnope({
      load: [Bundles.jQuery()].concat(Bundles.Chiffon())
      , complete: function() {
        var Chiffon = win.Chiffon;
        if (undef === Chiffon) { return; }

        var context = {
          locale: locale
        };

        fn(new Chiffon(context));
      }
    });
  };

})(this, this.Dependencies, this.yepnope);
