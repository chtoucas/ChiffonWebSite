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

(function(window) {
  var scripts = [
    'vendor/jquery.cookie-1.3.1.js'
    , 'vendor/l10n-2013-04-18.js'
    , 'localization.js'
    , 'chiffon.js'
  ].map(function(file) { return 'assets/jsdebug/' + file; });

  yepnope([{
    // Google Analytics.
    test: window._gaq || false
    , yep: '//www.google-analytics.com/ga.js'
  }, {
    test: '__proto__' in {}
    , yep: 'assets/jsdebug/vendor/zepto-1.0.1.js'
    , nope: 'assets/jsdebug/vendor/jquery-1.10.2.js'
    , callback: function(url, result, key) {
      if (!window.jQuery) { window.jQuery = window.Zepto; }
    }
  }, {
    // Main.
    load: scripts
    , complete: function() { Chiffon.home(); }
  }]);
})(this);
