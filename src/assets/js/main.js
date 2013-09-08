;

this.Env = (function(undef) {
  'use strict';

  var Env = {}, locales = ['en', 'fr'];

  function _Env() { }

  Env.Current = undef;

  Env.Init = function(version, authenticated, locale, debug, baseUrl) {
    if (undef === version) {
      throw new ReferenceError('The "version" argument is undefined.');
    }

    if (-1 === locales.indexOf(locale)) {
      throw new Error('The locale "' + locale + '" is not supported.');
    }

    if (undef === baseUrl) {
      throw new ReferenceError('The "baseUrl" argument is undefined.');
    } else if ('/' !== baseUrl.substring(-1, 1)) {
      baseUrl = baseUrl + '/';
    }

    var env = new _Env();

    env.baseUrl = baseUrl;
    env.debug = true === debug;
    env.locale = locale;
    env.user = { authenticated: true === authenticated };
    env.version = version;

    Env.Current = env;
  };

  return Env;
})();

this.Dependencies = (function() {
  'use strict';

  function Dependencies(env) {
    var baseUrl = env.baseUrl;

    this.debug = env.debug;
    this.locale = env.locale;
    this.version = env.version;

    this.rebase = function(src) { return baseUrl + src; }

    this.vendor = function(src) { return baseUrl + 'vendor/' + src; }
  }

  Dependencies.prototype = {
    jQuery: function() {
      return this.vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js');
    }

    //, jQueryCookie: function() { return this.vendor('jquery.cookie-1.3.1.min.js'); }

    //, jQueryOutside: function() { return this.vendor('jquery.ba-outside-events-1.1.min.js'); }

    , jQueryValidate: function() {
      return ['jquery.validate.min.js', 'additional-methods.min.js']
        .push('localization/messages_' + this.locale + '.js')
        .map(function(src) { return this.vendor('jquery.validate-1.11.1/' + src); });
    }

    , Chiffon: function() {
      return this.debug
        ? ['vendor/l10n-2013.04.18.min.js', 'localization.js', 'chiffon.js'].map(this.rebase)
        : rebase('chiffon-' + this.version + '.min.js');;
    }
  };

  return Dependencies;
})();

this.main = (function(window, Env, Dependencies, yepnope, undef) {
  'use strict';

  return function(fn) {
    var deps, env = Env.Current;

    if (undef === env) { throw new Error('The "Env" variable is not initialized.'); }
    deps = new Dependencies(env);
    // FIXME: Quid quand un des appels échoue ?
    yepnope({
      load: [deps.jQuery()].concat(deps.Chiffon())
      , complete: function() {
        var Chiffon = window.Chiffon;
        if (undef === Chiffon) { return; }
        fn(new Chiffon(env, deps));
      }
    });
  };

})(this, this.Env, this.Dependencies, this.yepnope);
