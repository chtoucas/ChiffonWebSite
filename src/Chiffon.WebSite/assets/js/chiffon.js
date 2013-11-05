/*jshint laxcomma: true*/
/*jslint nomen:true, white: true, todo: true*/
/// <reference path="app.ts"/>
var Chiffon;
(function (Chiffon) {
    'use strict';

    // Configuration globale du comportement des appels Ajax via jQuery.
    function setupAjax(timeout) {
        $.ajaxSetup({
            timeout: timeout,
            async: true,
            cache: true
        });
    }

    // Configuration de L10N.
    function setLocale(locale) {
        String.locale = locale;
    }

    var App = (function () {
        function App(ctx) {
            this.ctx = ctx;
        }
        App.prototype.handleCore = function (controllerName, actionName, params) {
            throw new Error('You must override the "handleCore" method.');
        };

        App.prototype.handle = function (controllerName, actionName, params) {
            setupAjax(Chiffon.defaults.ajaxTimeout);
            setLocale(this.ctx.locale);

            this.handleCore(controllerName, actionName, params);
        };
        return App;
    })();
    Chiffon.App = App;
})(Chiffon || (Chiffon = {}));

this.Chiffon.Views = ((function (win, doc, loc, $) {
    'use strict';

    var Views = {
        Layout: null,
        DesignerLayout: null,
        StickyInfo: null,
        HomeIndex: null,
        AccountLogin: null,
        AccountRegister: null,
        DesignerPattern: null,
        ViewNavigator: null
    };

    // L10N
    function l(string) {
        return string.toLocaleString();
    }

    // Chargement de jQuery.validate puis exécution d'un callback.
    function initializeValidation(ctx, fn) {
        var app = ctx.app;

        app.require(app.dependencies.jQueryValidate(ctx.locale), function () {
            var $errContainer = $('#error_container');

            $.validator.setDefaults({
                hightlight: function (elmt) {
                    $(elmt).addClass('error');
                },
                unhightlight: function (elmt) {
                    $(elmt).removeClass('error');
                },
                errorContainer: $errContainer,
                errorLabelContainer: $errContainer,
                invalidHandler: function () {
                    $errContainer.fadeIn();
                }
            });

            if (null !== fn) {
                fn();
            }
        });
    }

    /* BaseView */
    function BaseView(ctx) {
        this.ctx = ctx;
    }

    /* Layouts */
    Views.Layout = ((function () {
        function Layout(ctx) {
            BaseView.apply(this, arguments);
        }

        Layout.prototype = {
            initialize: function () {
                // On ouvre les liens externes dans une nouvelle fenêtre.
                $('A[rel=external]').external();

                if (this.ctx.user.isAnonymous) {
                    // NB: On place l'événement sur "doc" car on veut rester dans la modale après un clic.
                    $(doc).on('click.modal', 'A[rel="modal:open"]', function (e) {
                        e.preventDefault();

                        $(this).modal({ closeText: l('%modal.close') });
                    });
                }
            }
        };

        return Layout;
    })());

    Views.DesignerLayout = ((function () {
        function DesignerLayout(ctx) {
            BaseView.apply(this, arguments);
            this.layoutView = new Views.Layout(ctx);
        }

        DesignerLayout.prototype = {
            initialize: function () {
                this.layoutView.initialize();
                Views.StickyInfo();
            }
        };

        return DesignerLayout;
    })());

    function DefaultView(ctx) {
        BaseView.apply(this, arguments);
        this.layoutView = new Views.Layout(ctx);
    }

    DefaultView.prototype = {
        initialize: function () {
            this.layoutView.initialize();
        }
    };

    function DesignerView(ctx) {
        BaseView.apply(this, arguments);
        this.layoutView = new Views.DesignerLayout(ctx);
    }

    DesignerView.prototype = {
        initialize: function () {
            this.layoutView.initialize();
        }
    };

    /* Pages Home */
    Views.HomeIndex = ((function () {
        function HomeIndex(ctx) {
            DefaultView.apply(this, arguments);
        }

        HomeIndex.prototype = {
            initialize: function () {
                DefaultView.prototype.initialize.call(this);

                $('.vignette').watermark(l('%preview.watermark'));
            }
        };

        return HomeIndex;
    })());

    /* Pages Contact */
    Views.AccountLogin = ((function () {
        function AccountLogin(ctx) {
            DefaultView.apply(this, arguments);
        }

        AccountLogin.prototype = {
            initialize: function () {
                DefaultView.prototype.initialize.call(this);

                initializeValidation(this.ctx, function () {
                    $('#login_form').validate({
                        messages: { token: l('%login.password_required') },
                        rules: { token: { required: true, minlength: 10 } }
                    });
                });
            }
        };

        return AccountLogin;
    })());

    Views.AccountRegister = ((function () {
        function AccountRegister(ctx) {
            DefaultView.apply(this, arguments);
        }

        AccountRegister.prototype = {
            initialize: function () {
                DefaultView.prototype.initialize.call(this);

                var $form = $('#register_form');

                initializeValidation(this.ctx, function () {
                    $form.validate({
                        // NB: On ne veut pas de message d'erreur par "input".
                        // TODO: "errorPlacement" ne semble pas être la bonne méthode à utiliser.
                        errorPlacement: $.noop,
                        messages: {
                            Lastname: '',
                            Firstname: '',
                            CompanyName: '',
                            EmailAddress: ''
                        },
                        rules: {
                            Lastname: { required: true, minlength: 2, maxlength: 50 },
                            Firstname: { required: true, minlength: 2, maxlength: 50 },
                            CompanyName: { required: true, minlength: 2, maxlength: 100 },
                            EmailAddress: { required: true, minlength: 5, maxlength: 200 },
                            Message: { maxlength: 4000 }
                        }
                    });
                });
            }
        };

        return AccountRegister;
    })());

    /* Pages Designer */
    Views.DesignerPattern = ((function () {
        function DesignerPattern(ctx) {
            DesignerView.apply(this, arguments);
        }

        DesignerPattern.prototype = {
            initialize: function () {
                DesignerView.prototype.initialize.call(this);

                // NB: location.hash contient le caractère '#'.
                Views.ViewNavigator({ currentSel: loc.hash });
            }
        };

        return DesignerPattern;
    })());

    /* Composants communs */
    // Navigation entre les vues d'un même motif.
    Views.ViewNavigator = ((function () {
        var settings, defaults = {
            viewsSel: '.pattern',
            navSel: '#nav_views',
            currentSel: null
        }, $views, $prev, $prev_noop, $next, $next_noop, $pos, pos = 1, length, selectors = [null];

        /* Actions sur l'objet $views */
        // Rend visible la vue à la position 'i' et cache les autres vues.
        function showViewAt(i) {
            $views.hide();
            $(selectors[i]).fadeIn();
        }

        /* Actions sur l'objet $pos */
        // Met à jour l'indicateur visuel de position quand on est à la position 'i'.
        function setPositionMarkAt(i) {
            $pos.html(i);
        }

        /* Actions sur les objets $prev et $prev_noop */
        // Désactive le lien 'Précédent'.
        function disablePreviousLink() {
            $prev.hide();
            $prev_noop.show();
        }

        // Active le lien 'Précédent'.
        function enablePreviousLink() {
            $prev.show();
            $prev_noop.hide();
        }

        // Met à jour le lien 'Précédent' quand on arrive à la position 'i'.
        function setPreviousLinkAt(i) {
            $prev.attr('href', selectors[i - 1]);
        }

        /* Actions sur les objets $next et $next_noop */
        // Désactive le lien 'Suivant'.
        function disableNextLink() {
            $next.hide();
            $next_noop.show();
        }

        // Active le lien 'Suivant'.
        function enableNextLink() {
            $next.show();
            $next_noop.hide();
        }

        // Met à jour le lien 'Suivant' quand on arrive à la position 'i'.
        function setNextLinkAt(i) {
            $next.attr('href', selectors[i + 1]);
        }

        /* Utilitaires */
        function getSelector(obj) {
            // FIXME: prop ou attr ?
            return '#' + $(obj).prop('id');
        }

        function goTo(i) {
            showViewAt(i);
            setPositionMarkAt(i);
        }

        /* Fonctions utilisées lors d'un changement de position */
        // Actions à mener lors d'un démarrage à la position 'i'.
        function startAt(i) {
            // TODO: Vérifier que le motif présenté par défaut est bien visible au démarrage
            // et, donc, qu'on n'ait pas besoin d'appeler showViewAt(i).
            setPositionMarkAt(i);

            // On active le lien 'Précédent' car, au démarrage, il est invisible.
            setPreviousLinkAt(i);
            enablePreviousLink();

            if (length === i) {
                // En dernière position, on désactive le lien 'Suivant'.
                disableNextLink();
            } else {
                setNextLinkAt(i);
            }
        }

        // Actions à mener quand on avance à la position 'i'.
        function goBackTo(i) {
            goTo(i);

            if (1 === i) {
                // En première position, on désactive le lien 'Précédent'.
                disablePreviousLink();
            } else {
                setPreviousLinkAt(i);
            }

            if (length !== i) {
                // Si on n'est pas en dernière position, on met à jour le lien 'Suivant'.
                setNextLinkAt(i);

                if (length - 1 === i) {
                    // En avant-dernière position, on active le lien 'Suivant'.
                    enableNextLink();
                }
            }
        }

        // Actions à mener quand on recule à la position 'i'.
        function goForthTo(i) {
            goTo(i);

            if (1 !== i) {
                // Si on n'est pas en première position, on met à jour le lien 'Précédent'.
                setPreviousLinkAt(i);

                if (2 === i) {
                    // En deuxième position, on active le lien 'Précédent'.
                    enablePreviousLink();
                }
            }

            if (length === i) {
                // En dernière position, on désactive le lien 'Suivant'.
                disableNextLink();
            } else {
                setNextLinkAt(i);
            }
        }

        /* Configuration et initialisation */
        function setupLinkHandlers() {
            $prev.click(function (e) {
                e.preventDefault();

                goBackTo(--pos);
            });
            $next.click(function (e) {
                e.preventDefault();

                goForthTo(++pos);
            });
        }

        function initialize() {
            var $nav, currentSel, callback;

            $views = $(settings.viewsSel);
            length = $views.length;

            if (length <= 1) {
                // Si on a au plus une vue, pas besoin d'aller plus loin.
                return;
            }

            $nav = $(settings.navSel);
            $prev = $nav.find('.prev');
            $prev_noop = $nav.find('.prev_noop');
            $next = $nav.find('.next');
            $next_noop = $nav.find('.next_noop');
            $pos = $nav.find('.pos');

            currentSel = settings.currentSel;

            if (null === currentSel) {
                callback = function () {
                    selectors.push(getSelector(this));
                };
            } else {
                callback = function (i) {
                    var sel = getSelector(this);
                    if (currentSel === sel) {
                        // On recherche la position initiale.
                        pos = i + 1;
                    }
                    selectors.push(sel);
                };
            }

            $views.each(callback);
        }

        return function (options) {
            settings = _.defaults(options || {}, defaults);

            initialize();

            if (length <= 1) {
                return;
            }

            if (pos > 1) {
                startAt(pos);
            }
            setupLinkHandlers();
        };
    })());

    // Autant que possible on s'assure que le bloc informations sur le designer est toujours visible.
    Views.StickyInfo = ((function () {
        // Configuration par défaut.
        var settings, defaults = {
            infoSel: '#info',
            designerSel: '#designer'
        }, $info, $designer, info_h, info_w, info_top, info_left, designer_w, window_h;

        function handleScrollEventForMediumBlock() {
            var scroll_limit, sticky_class = 'sticky top', is_sticky = false;

            function stick() {
                if (is_sticky) {
                    return;
                }
                is_sticky = true;
                $info.addClass(sticky_class);
            }

            function unstick() {
                if (!is_sticky) {
                    return;
                }
                is_sticky = false;
                $info.removeClass(sticky_class);
            }

            scroll_limit = info_top - 23;

            if ($(win).scrollTop() >= scroll_limit) {
                stick();
            }

            $(win).scroll(function () {
                if ($(win).scrollTop() >= scroll_limit) {
                    stick();
                } else {
                    unstick();
                }
            });
        }

        // FIXME: Pour le moment, on ne s'occupe que des redimensionnements horizontaux.
        function handleResizeEvent() {
            // On utilise "_.debounce()" pour temporiser la prise en charge de l'évènement "resize"
            // pendant 150 millisecondes.
            $(win).resize(_.debounce(function () {
                var left = $designer.offset().left + designer_w - info_w;

                $info.css({ 'left': left + 'px' });
            }, 150));
        }

        // Dans sa position initiale, le bloc info est entièrement contenu dans la fenêtre ;
        // pour qu'il soit toujours visible on lui donne une position fixe.
        function setupSmallBlock() {
            $info.addClass('sticky');
            $info.css({ top: info_top + 'px', left: info_left + 'px' });
        }

        // La fenêtre peut contenir tout le bloc info, mais à condition de le positioner tout en
        // haut de la fenêtre.
        function setupMediumBlock() {
            // On applique la propriété 'left' uniquement au chargement car elle pourrait être modifiée
            // plus tard lors d'un redimensionnement horizontal de la fenêtre.
            $info.css('left', info_left + 'px');

            handleScrollEventForMediumBlock();
        }

        function initialize() {
            var info_offset;

            $info = $(settings.infoSel);
            $designer = $(settings.designerSel);

            info_h = $info.height();
            info_w = $info.width();

            info_offset = $info.offset();
            info_top = info_offset.top;
            info_left = info_offset.left;

            designer_w = $designer.width();
            window_h = $(win).height();
        }

        return function (options) {
            settings = _.defaults(options || {}, defaults);

            initialize();

            if (window_h >= info_h + info_top) {
                setupSmallBlock();
            } else if (window_h >= info_h) {
                setupMediumBlock();
            } else {
                // La fenêtre est trop petite pour contenir tout le bloc info.
                // Si on donne une position fixe à ce dernier, le contenu en bas n'est jamais visible.
                // On ne touche donc à rien.
                return;
            }

            handleResizeEvent();
        };
    })());

    return Views;
})(this, this.document, this.location, jQuery));

this.Chiffon.Controllers = ((function ($, Views) {
    'use strict';

    function extend(methods) {
        return $.extend({}, BaseController.prototype, methods);
    }

    /* BaseController */
    function BaseController(ctx) {
        this.ctx = ctx;
    }

    /* ContactController */
    function AccountController() {
        BaseController.apply(this, arguments);
    }

    AccountController.prototype = extend({
        login: function () {
            (new Views.AccountLogin(this.ctx)).initialize();
        },
        newsletter: function () {
            (new Views.Layout(this.ctx)).initialize();
        },
        register: function () {
            (new Views.AccountRegister(this.ctx)).initialize();
        }
    });

    /* DesignerController */
    function DesignerController() {
        BaseController.apply(this, arguments);
    }

    DesignerController.prototype = extend({
        index: function () {
            (new Views.DesignerLayout(this.ctx)).initialize();
        },
        category: function () {
            (new Views.DesignerLayout(this.ctx)).initialize();
        },
        pattern: function () {
            (new Views.DesignerPattern(this.ctx)).initialize();
        }
    });

    /* HomeController */
    function HomeController() {
        BaseController.apply(this, arguments);
    }

    HomeController.prototype = extend({
        about: function () {
            (new Views.Layout(this.ctx)).initialize();
        },
        contact: function () {
            (new Views.Layout(this.ctx)).initialize();
        },
        index: function () {
            (new Views.HomeIndex(this.ctx)).initialize();
        }
    });

    return {
        Account: AccountController,
        Designer: DesignerController,
        Home: HomeController
    };
})(jQuery, this.Chiffon.Views));

this.Chiffon.prototype.handleCore = ((function (Controllers) {
    'use strict';

    return function (controllerName, actionName, params) {
        if (Controllers.hasOwnProperty(controllerName)) {
            var controller, ControllerClass = Controllers[controllerName], controllerPrototype = ControllerClass.prototype, actionMethod = actionName.toLowerCase();

            if (controllerPrototype.hasOwnProperty(actionMethod)) {
                controller = new ControllerClass(this.ctx);
                controllerPrototype[actionMethod].apply(controller, params);
            }
        }
    };
})(this.Chiffon.Controllers));
//# sourceMappingURL=chiffon.js.map
