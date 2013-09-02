// ECMA-5 map function, if not available.
if (!Array.prototype.map) {
  Array.prototype.map = function(fun /*, thisp */) {
    var len = this.length >>> 0
      , res = new Array(len)
      , thisp = arguments[1];

    for (var i = 0; i < len; i++) {
      if (i in this) {
        res[i] = fun.call(thisp, this[i], i, this);
      }
    }
    return res;
  };
}

var getChiffon = (function(window) {
  'use strict';

  var _fmap = {
    debug: {
      jquery: 'vendor/jquery-1.10.2.min.js'
      , jquery2: 'vendor/jquery-2.0.3.min.js'
      , main: [
        //'vendor/jquery.cookie-1.3.1.js'
        //, 'vendor/jquery.ba-outside-events-1.1.js'
        //, 'vendor/jquery.validate-1.11.1/jquery.validate.js'
        //, 'vendor/jquery.validate-1.11.1/additional-methods.js'
        'vendor/l10n-2013.04.18.min.js'
        , 'localization.js'
        , 'chiffon.js'
      ]
    }
    , release: {
      jquery: 'vendor/jquery-1.10.2.min.js'
      , jquery2: 'vendor/jquery-2.0.3.min.js'
      , main: ['chi.min.js']
    }
  };

  var ctor, Chiffon = {};

  ctor = function(debug, baseUrl) {
    var that = Chiffon;

    that.debug = debug;
    that.baseUrl = baseUrl;
    that.fmap = debug ? _fmap.debug : _fmap.release;
    //that.realurl = function(src) { return baseUrl + src; };

    return that;
  }

  Chiffon.realurl = function(src) { return Chiffon.baseUrl + src; };

  Chiffon.main = function(fn, options) {
    var that = this
      , fmap = that.fmap
      , realurl = that.realurl;

    yepnope([{
      test: '__proto__' in {}
      , yep: realurl(fmap.jquery2)
      , nope: realurl(fmap.jquery)
    }
    , {
      load: fmap.main.map(realurl)
      , complete: function() { $(function() { that.configure(options); fn(that); }); }
    }]);
  };

  return ctor;
})(this);

