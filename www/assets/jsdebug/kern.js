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

var Kern = (function(window) {
  'use strict';

  return {
    boot: function(debug, baseUrl, route, opts) {
      var _realpath = function(src) { return baseUrl + src; }

        , _scripts = (
          debug
          ? [
              'vendor/jquery.cookie-1.3.1.js'
              , 'vendor/l10n-2013.04.18.min.js'
              , 'localization.js'
              , 'chiffon.js'
          ]
          : ['vendor-1.0.js', 'chiffon-1.0.js']
        ).map(_realpath);

      self.realpath = _realpath

      yepnope([{
        // jQuery or Zepto.
        test: '__proto__' in {}
        , yep: { 'zepto': _realpath('vendor/zepto-1.0.1.min.js') }
        , nope: { 'jquery': _realpath('vendor/jquery-1.10.2.min.js') }
        , callback: function(url, result, key) {
          if (key === 'zepto') { window.jQuery = window.$; }
        }
      }
      , {
        // Main.
        load: _scripts
        , complete: function() { Chiffon.main(route, opts); }
      }]);
    }
  };
})(this);

