
function l(string) {
  return string.toLocaleString();
}

export class User {
  constructor(public isAuth: boolean) { }

  get isAnonymous() {
    return !this.isAuth;
  }
}

export class Context {
  public locale: string;
  public user: User;
  public app: any;
}

class Chiffon {
  private static defaults = {
    ajaxTimeout: 3000
  };

  constructor(public ctx: Context) { }

  handleCore(controllerName: string, actionName: string, params: any[]): void {
    throw new Error('You must override the "handleCore" method.');
  }

  handle(controllerName: string, actionName: string, params: any[]): void {
    this._setupAjax(Chiffon.defaults.ajaxTimeout);
    this._setLocale(this.ctx.locale);

    this.handleCore(controllerName, actionName, params);
  }

  private _setLocale(locale): void {
    //String.locale = locale;
  }

  // Configuration globale du comportement des appels Ajax via jQuery.
  private _setupAjax(timeout): void {
    $.ajaxSetup({
      timeout: timeout
      , async: true
      , cache: true
    });
  }
}

module Chiffon {
  export module Views {
    export interface IView {
      initialize(): void;
    }

    // Classe de base utilisée par toutes les pages.
    export class BaseView implements IView {
      constructor(public ctx: Context) { }

      initialize() { throw new Error('You must override the "initialize" method.'); }
    }

    // Layouts.
    export class Layout extends BaseView {
      constructor(public ctx: Context) {
        super(ctx);
      }

      initialize() {
        // On ouvre les liens externes dans une nouvelle fenêtre.
        $('A[rel=external]').external();

        // Pour les visiteurs anonymes uniquement, on active les modales.
        if (this.ctx.user.isAnonymous) {
          // NB: On place l'événement sur "doc" car on veut rester dans la modale après un clic.
          $(document).on('click.modal', 'A[rel="modal:open"]', function (e) {
            e.preventDefault();

            $(this).modal({ closeText: l('%modal.close') });
          });
        } /* else {
          // TODO: Utiliser un hashcode pour afficher la confirmation de compte.
          //$('<div class="welcome serif serif_large"><h2>Bienvenue !</h2><p>Merci de vous être inscrit.</p></div>')
          //  .appendTo('body').modal({ closeText: l('%modal.close') });
        } */
      }
    }

    export class DesignerLayout extends Layout {
      constructor(public ctx: Context) {
        super(ctx);
      }

      initialize() {
        super.initialize();
      }
    }

    // Vue par défaut.
    export class DefaultView extends BaseView {
      private _layoutView: IView;

      constructor(public ctx: Context) {
        super(ctx);
        this._layoutView = new Layout(ctx);
      }

      initalize() {
        this._layoutView.initialize();
      }
    }

    export class AccountLogin extends DefaultView {
      constructor(public ctx: Context) {
        super(ctx);
      }
    }

    export class AccountRegister extends DefaultView {
      constructor(public ctx: Context) {
        super(ctx);
      }
    }
  }

  export module Controllers {
    export class BaseController {
      constructor(public ctx: Context) { }
    }

    export class AccountController extends BaseController {
      constructor(public ctx: Context) { super(ctx); }

      login() { (new Views.AccountLogin(this.ctx)).initialize(); }
      newsletter() { (new Views.Layout(this.ctx)).initialize(); }
      register() { (new Views.AccountRegister(this.ctx)).initialize(); }
    }

    export class DesignerController extends BaseController {
      constructor(public ctx: Context) { super(ctx); }

      index() { (new Views.DesignerLayout(this.ctx)).initialize(); }
      category() { (new Views.DesignerLayout(this.ctx)).initialize(); }
      //pattern() { (new Chiffon.Views.DesignerPattern(this.ctx)).initialize(); }
    }
  }
}
