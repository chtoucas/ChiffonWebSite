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

  var _fmap = {
    debug: {
      jquery: 'vendor/jquery-1.10.2.js'
      , zepto: 'vendor/zepto-1.0.1.js'
      , main: [
          'vendor/jquery.cookie-1.3.1.js'
          , 'vendor/l10n-2013.04.18.js'
          , 'localization.js'
          , 'chiffon.js'
        ]
    }
    , release: {
      jquery: 'vendor/jquery-1.10.2.min.js'
      , zepto: 'vendor/zepto-1.0.1.min.js'
      , main: ['vendor-1.0.js', 'chiffon-1.0.js']
    }
  };

  var self = {};

  self.boot: function(debug, baseUrl, route, opts) {
    var fmap = self.fmap = debug ? _fmap.debug : _fmap.release;
    var realurl = self.realurl = function(src) { return baseUrl + src; };

    yepnope([{
      // jQuery or Zepto.
      test: '__proto__' in {}
      // XXX, yep: { 'zepto': realpath(map.zepto) }
      , yep: { 'jquery': realurl(map.jquery) }
      , nope: { 'jquery': realurl(map.jquery) }
      , callback: function(url, result, key) {
        if (key === 'zepto') { window.jQuery = window.$; }
      }
    }
    , {
      // Main.
      load: fmap.main.map(realurl)
      , complete: function() { $(function() { Chiffon.main(route, opts); }); }
    }]);
  };

  return self;
})(this);

