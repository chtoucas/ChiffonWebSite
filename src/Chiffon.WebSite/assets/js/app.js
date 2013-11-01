/*jshint laxcomma: true, laxbreak:true*/
/*jslint nomen:true, white: true, todo: true*/

this.App = (function(win, _, yepnope, undef) {
  'use strict';

  // Configuration par défaut.
  var defaults = {
      baseUrl: undef
      , debug: false
      , version: undef
    }
    // Langues supportées.
    , locales = ['fr', 'en']

    , loadJS = function(/* options */) {
      throw new Error('You can not use this method until the main() method has been called.');
    };

  return function(options) {
    var settings = _.defaults(options || {}, defaults);

    if (undef === settings.baseUrl) {
      throw new Error('The baseUrl is not defined.');
    }
    if ('/' !== settings.baseUrl.substring(-1, 1)) {
      settings.baseUrl = settings.baseUrl + '/';
    }

    function rebase(src) { return settings.baseUrl + src; }

    function vendor(src) { return settings.baseUrl + 'vendor/' + src; }

    this.dependencies = {
      chiffon: function() {
        return settings.debug
          ? ['jquery.plugins.js', 'vendor/l10n-2013.09.12.min.js', 'localization.js', 'chiffon.js'].map(rebase)
          : rebase('chiffon-' + settings.version + '.min.js');
      }

      , jQuery: function() {
        return [vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js')];
      }

      // TODO: Minification.
      //, jQueryCookie: function() { return vendor('jquery.cookie-1.3.1.js'); }

      //, jQueryOutside: function() { return vendor('jquery.ba-outside-events-1.1.min.js'); }

      , jQueryValidate: function(locale) {
        var scripts = ['jquery.validate.min.js'];
        if ('en' !== locale) { scripts.push('localization/messages_' + locale + '.js'); }
        return scripts.map(function(src) { return vendor('jquery.validate-1.11.1/' + src); });
      }
    };

    this.require = function(dependencies, onComplete) {
      loadJS({
        load: dependencies
        , complete: onComplete
      });
    };

    this.main = function(isAuthenticated, locale, fn) {
      var that = this
        , isAuth = true === isAuthenticated;

      if (-1 === locales.indexOf(locale)) {
        throw new Error('The locale "' + locale + '" is not supported.');
      }

      // TODO: Désactiver la suite pour les petits écrans ?
      // Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html

      yepnope({
        load: that.dependencies.jQuery()
        , complete: function() {
          // Si le chargement de jQuery a échoué, on dégage.
          if (undef === win.jQuery) { return; }

          yepnope({
            load: that.dependencies.chiffon()
            , complete: function() {
              var Chiffon = win.Chiffon;
              if (undef === Chiffon) { return; }

              loadJS = function(options) { yepnope(options); };

              var ctx = {
                locale: locale
                , app: that
                , user: {
                  isAnonymous: !isAuth
                  , isAuth: isAuth
                }
              };

              fn(new Chiffon(ctx));
            }
          });
        }
      });
    };
  };

}(this, this._, this.yepnope));
