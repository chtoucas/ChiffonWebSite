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
      , jquery2: 'vendor/jquery-2.0.3.js'
      //, zepto: 'vendor/zepto-1.0.1.js'
      , main: [
          'vendor/jquery.cookie-1.3.1.js'
          , 'vendor/l10n-2013.04.18.js'
          , 'localization.js'
          , 'chiffon.js'
      ]
    }
    , release: {
      jquery: 'vendor/jquery-1.10.2.min.js'
      , jquery2: 'vendor/jquery-2.0.3.min.js'
      //, zepto: 'vendor/zepto-1.0.1.min.js'
      , main: ['vendor-1.0.js', 'chiffon-1.0.js']
    }
  };

  var kern = {};

  kern.boot = function(debug, baseUrl, route, params) {
    var fmap = kern.fmap = debug ? _fmap.debug : _fmap.release;
    var realurl = kern.realurl = function(src) { return baseUrl + src; };

    yepnope([{
      test: '__proto__' in {}
      // jQuery v1 or v2
      , yep: realurl(fmap.jquery2)
      , nope: realurl(fmap.jquery)
      // jQuery or Zepto.
      //, yep: { 'zepto': realurl(fmap.zepto) }
      //, nope: { 'jquery': realurl(fmap.jquery) }
      //, callback: function(url, result, key) {
      //  if (key === 'zepto') { window.jQuery = window.$; }
      //}
    }
    , {
      // Main.
      load: fmap.main.map(realurl)
      , complete: function() { $(function() { Chiffon.make().handle(route, params); }); }
    }]);
  };

  return kern;
})(this);

