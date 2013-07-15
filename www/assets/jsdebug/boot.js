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
        'vendor/jquery-1.9.1.min.js'
      , 'vendor/jquery.cookie.js'
      , 'console.js'
      , 'common.js'
    ]
    .map(function(file) { return 'assets/jsdebug/' + file; });

  yepnope([{
    // Google Analytics.
    test: window._gaq || false,
    yep: '//www.google-analytics.com/ga.js'
  }, {
    // Main.
    load: scripts,
    complete: function() { App.main(); }
  }]);
})(this);
