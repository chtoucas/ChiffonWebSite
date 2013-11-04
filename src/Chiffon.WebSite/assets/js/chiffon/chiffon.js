var __extends = this.__extends || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    __.prototype = b.prototype;
    d.prototype = new __();
};
define(["require", "exports"], function(require, exports) {
    function l(string) {
        return string.toLocaleString();
    }

    var User = (function () {
        function User(isAuth) {
            this.isAuth = isAuth;
        }
        Object.defineProperty(User.prototype, "isAnonymous", {
            get: function () {
                return !this.isAuth;
            },
            enumerable: true,
            configurable: true
        });
        return User;
    })();
    exports.User = User;

    var Context = (function () {
        function Context() {
        }
        return Context;
    })();
    exports.Context = Context;

    var Chiffon = (function () {
        function Chiffon(ctx) {
            this.ctx = ctx;
        }
        Chiffon.prototype.handleCore = function (controllerName, actionName, params) {
            throw new Error('You must override the "handleCore" method.');
        };

        Chiffon.prototype.handle = function (controllerName, actionName, params) {
            this._setupAjax(Chiffon.defaults.ajaxTimeout);
            this._setLocale(this.ctx.locale);

            this.handleCore(controllerName, actionName, params);
        };

        Chiffon.prototype._setLocale = function (locale) {
            //String.locale = locale;
        };

        // Configuration globale du comportement des appels Ajax via jQuery.
        Chiffon.prototype._setupAjax = function (timeout) {
            $.ajaxSetup({
                timeout: timeout,
                async: true,
                cache: true
            });
        };
        Chiffon.defaults = {
            ajaxTimeout: 3000
        };
        return Chiffon;
    })();

    var Chiffon;
    (function (Chiffon) {
        (function (Views) {
            // Classe de base utilisée par toutes les pages.
            var BaseView = (function () {
                function BaseView(ctx) {
                    this.ctx = ctx;
                }
                BaseView.prototype.initialize = function () {
                    throw new Error('You must override the "initialize" method.');
                };
                return BaseView;
            })();
            Views.BaseView = BaseView;

            // Layouts.
            var Layout = (function (_super) {
                __extends(Layout, _super);
                function Layout(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                }
                Layout.prototype.initialize = function () {
                    // On ouvre les liens externes dans une nouvelle fenêtre.
                    $('A[rel=external]').external();

                    if (this.ctx.user.isAnonymous) {
                        // NB: On place l'événement sur "doc" car on veut rester dans la modale après un clic.
                        $(document).on('click.modal', 'A[rel="modal:open"]', function (e) {
                            e.preventDefault();

                            $(this).modal({ closeText: l('%modal.close') });
                        });
                    }
                };
                return Layout;
            })(BaseView);
            Views.Layout = Layout;

            var DesignerLayout = (function (_super) {
                __extends(DesignerLayout, _super);
                function DesignerLayout(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                }
                DesignerLayout.prototype.initialize = function () {
                    _super.prototype.initialize.call(this);
                };
                return DesignerLayout;
            })(Layout);
            Views.DesignerLayout = DesignerLayout;

            // Vue par défaut.
            var DefaultView = (function (_super) {
                __extends(DefaultView, _super);
                function DefaultView(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                    this._layoutView = new Layout(ctx);
                }
                DefaultView.prototype.initalize = function () {
                    this._layoutView.initialize();
                };
                return DefaultView;
            })(BaseView);
            Views.DefaultView = DefaultView;

            var AccountLogin = (function (_super) {
                __extends(AccountLogin, _super);
                function AccountLogin(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                }
                return AccountLogin;
            })(DefaultView);
            Views.AccountLogin = AccountLogin;

            var AccountRegister = (function (_super) {
                __extends(AccountRegister, _super);
                function AccountRegister(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                }
                return AccountRegister;
            })(DefaultView);
            Views.AccountRegister = AccountRegister;
        })(Chiffon.Views || (Chiffon.Views = {}));
        var Views = Chiffon.Views;

        (function (Controllers) {
            var BaseController = (function () {
                function BaseController(ctx) {
                    this.ctx = ctx;
                }
                return BaseController;
            })();
            Controllers.BaseController = BaseController;

            var AccountController = (function (_super) {
                __extends(AccountController, _super);
                function AccountController(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                }
                AccountController.prototype.login = function () {
                    (new Views.AccountLogin(this.ctx)).initialize();
                };
                AccountController.prototype.newsletter = function () {
                    (new Views.Layout(this.ctx)).initialize();
                };
                AccountController.prototype.register = function () {
                    (new Views.AccountRegister(this.ctx)).initialize();
                };
                return AccountController;
            })(BaseController);
            Controllers.AccountController = AccountController;

            var DesignerController = (function (_super) {
                __extends(DesignerController, _super);
                function DesignerController(ctx) {
                    _super.call(this, ctx);
                    this.ctx = ctx;
                }
                DesignerController.prototype.index = function () {
                    (new Views.DesignerLayout(this.ctx)).initialize();
                };
                DesignerController.prototype.category = function () {
                    (new Views.DesignerLayout(this.ctx)).initialize();
                };
                return DesignerController;
            })(BaseController);
            Controllers.DesignerController = DesignerController;
        })(Chiffon.Controllers || (Chiffon.Controllers = {}));
        var Controllers = Chiffon.Controllers;
    })(Chiffon || (Chiffon = {}));
});
//# sourceMappingURL=chiffon.js.map
