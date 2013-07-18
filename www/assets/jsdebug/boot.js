// ECMA-5 map function, if not available.
if (!Array.prototype.map) {
  Array.prototype.map = function(fun /*, thisp */) {
    var len = this.length >>> 0;
    var res = new Array(len);
    var thisp = arguments[1];

    for (var i = 0; i < len; i++) {
      if (i in this) {
        res[i] = fun.call(thisp, this[i], i, this);
      }
    }
    return res;
  };
}

var versionize = (function(window, config) {
  var postfix = '-' + config.version + '.js';

  return function(src) {
    return src + postfix;
  };
})(this, config);

var realpath = (function(window, config) {
  var baseUrl = config.baseUrl + (config.debug ? 'jsdebug/' : 'js/');

  return function(src) {
    return baseUrl + src;
  };
})(this, config);

(function(window, config) {
  'use strict';

  var scripts = config.debug
    ? [
      'vendor/jquery.cookie-1.3.1.js'
      , 'vendor/l10n-2013.04.18.min.js'
      , 'localization.js'
      , 'chiffon.js'
    ]
    : [
      'vendor'
      , 'chiffon'
    ].map(versionize);

  yepnope([{
    // Google Analytics.
    // FIXME
    test: config.googleAnalytics
    , yep: '//www.google-analytics.com/ga.js'
  }, {
    // jQuery or Zepto.
    test: '__proto__' in {}
    , yep: realpath('vendor/zepto-1.0.1.min.js')
    , nope: realpath('vendor/jquery-1.10.2.min.js')
    , callback: function(url, result, key) {
      if (!window.jQuery) { window.jQuery = window.Zepto; }
    }
  }, {
    // Main.
    load: scripts.map(realpath)
    , complete: function() { Chiffon.main(); }
  }]);
})(this, config);
