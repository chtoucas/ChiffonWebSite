/*jshint eqeqeq:false, eqnull:true, bitwise:false, freeze:false, quotmark:false*/

// Borrowed from es5-shim.js.

(function () {
  'use strict';

  if (!Array.prototype.map) {
    var
      // Having a toString local variable name breaks in Opera so use _toString.
      _toString = Function.prototype.call.bind(Object.prototype.toString),

      // Check failure of by-index access of string characters (IE < 9)
      // and failure of `0 in boxedString` (Rhino)
      boxedString = Object("a"),
      splitString = boxedString[0] != "a" || !(0 in boxedString),

      // ES5 9.9
      // http://es5.github.com/#x9.9
      toObject = function (o) {
        if (o == null) { // this matches both null and undefined
          throw new TypeError("can't convert "+o+" to object");
        }
        return Object(o);
      };

    // ES5 15.4.4.19
    // http://es5.github.com/#x15.4.4.19
    // https://developer.mozilla.org/en/Core_JavaScript_1.5_Reference/Objects/Array/map

    Array.prototype.map = function map(fun /*, thisp*/) {
      var object = toObject(this),
        self = splitString
          && _toString(this) == "[object String]" ? this.split("") : object,
        length = self.length >>> 0,
        /*jshint -W064 */
        result = Array(length),
        /*jshint +W064 */
        thisp = arguments[1];

      // If no callback function or if callback is not a callable function
      if (_toString(fun) != "[object Function]") {
        throw new TypeError(fun + " is not a function");
      }

      for (var i = 0; i < length; i++) {
        if (i in self) {
          result[i] = fun.call(thisp, self[i], i, object);
        }
      }
      return result;
    };
  }
}());
