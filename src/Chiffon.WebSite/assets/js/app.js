/*jshint laxcomma: true, laxbreak:true*/
/*jslint nomen:true, white: true, todo: true*/
var Chiffon;
(function (Chiffon) {
    // Configuration par défaut.
    Chiffon.defaults = {
        ajaxTimeout: 3000
    };
})(Chiffon || (Chiffon = {}));

// Dépendances : _ et yepnope.
var App = ((function (win) {
    'use strict';

    // Configuration par défaut.
    var defaults = {
        baseUrl: null,
        debug: false,
        version: null
    }, locales = ['fr', 'en'];

    return function (options) {
        var settings = _.defaults(options || {}, defaults);

        if (null === settings.baseUrl) {
            throw new Error('The baseUrl is not defined.');
        }
        if ('/' !== settings.baseUrl.substring(-1, 1)) {
            settings.baseUrl = settings.baseUrl + '/';
        }

        function rebase(src) {
            return settings.baseUrl + src;
        }

        function vendor(src) {
            return settings.baseUrl + 'vendor/' + src;
        }

        this.dependencies = {
            chiffon: function () {
                var result = settings.debug ? [
                    'jquery.plugins.js',
                    'jquery.modal.js',
                    'vendor/l10n-2013.09.12.js',
                    'localization.js',
                    'chiffon.js'
                ].map(rebase) : [rebase('chiffon-' + settings.version + '.min.js')];
            },
            jQuery: function () {
                return [vendor('__proto__' in {} ? 'jquery-2.0.3.min.js' : 'jquery-1.10.2.min.js')];
            },
            jQueryValidate: function (locale) {
                var scripts = ['jquery.validate.min.js'];
                if ('en' !== locale) {
                    scripts.push('localization/messages_' + locale + '.js');
                }
                return scripts.map(function (src) {
                    return vendor('jquery.validate-1.11.1/' + src);
                });
            }
        };

        this.require = function (dependencies, onComplete) {
            yepnope({
                load: dependencies,
                complete: onComplete
            });
        };

        this.main = function (isAuthenticated, locale, fn) {
            var that = this, isAuth = true === isAuthenticated;

            if (-1 === locales.indexOf(locale)) {
                throw new Error('The locale "' + locale + '" is not supported.');
            }

            // TODO: Désactiver la suite pour les petits écrans ?
            // Cf. http://www.quirksmode.org/blog/archives/2012/03/windowouterwidt.html
            yepnope({
                load: that.dependencies.jQuery(),
                complete: function () {
                    if (null === win.jQuery) {
                        return;
                    }

                    yepnope({
                        load: that.dependencies.chiffon(),
                        complete: function () {
                            var App = Chiffon.App;
                            if (null === App) {
                                return;
                            }

                            var ctx = {
                                locale: locale,
                                app: that,
                                user: {
                                    isAnonymous: !isAuth,
                                    isAuth: isAuth
                                }
                            };

                            fn(new App(ctx));
                        }
                    });
                }
            });
        };
    };
})(this));
//# sourceMappingURL=app.js.map
