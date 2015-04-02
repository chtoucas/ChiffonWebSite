namespace Chiffon
{
    using System;
    using System.Diagnostics.Contracts;
    using System.Globalization;
    using System.Threading;
    using System.Web;
#if SHOWCASE
    using System.Web.SessionState;
#endif

    using Chiffon.Common;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    // TODO: Vérifier que ces événements ne sont déclenchés qu'une fois par requête.
    public sealed class InitializeContextModule : IHttpModule
    {
        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(InitializeContextModule));
        }

        public void Init(HttpApplication context)
        {
            Require.NotNull(context, "context");

            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute_;

#if SHOWCASE
            context.PostAcquireRequestState += OnPostAcquireRequestState_;
#else
            context.BeginRequest += OnBeginRequest_;
#endif
        }

        public void Dispose()
        {
            // Laissé vide intentionnellement.
        }

        // WARNING: Cette méthode ne convient pas avec les actions asynchrones 
        // car on peut changer de Thread.
        private static void InitializeCulture_(CultureInfo culture, CultureInfo uiCulture)
        {
            Contract.Requires(culture != null);
            Contract.Requires(uiCulture != null);

            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = culture;
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = uiCulture;
        }

#if SHOWCASE
        // NB: La session peut ne pas être disponible à ce moment.
        // http://stackoverflow.com/questions/276355/can-i-access-session-state-from-an-httpmodule
        private void OnPostAcquireRequestState_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;
            var request = context.Request;

            if (!(context.Handler is IRequiresSessionState))
            {
                // NB: Normalement, on ne devrait pas avoir à faire cette vérification,
                // mais cela peut poser des problèmes si il s'agit d'une requête prise
                // en charge par IIS en cas d'erreur (cf. httpErrors et customErrors dans
                // web.config).
                return;
            }

            var environment = ChiffonEnvironmentResolver.Resolve(request, app.Session);

            new ChiffonContext(environment).Register(context);
        }
#else
        private void OnBeginRequest_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            var environment = ChiffonEnvironmentResolver.Resolve(context.Request);

            new ChiffonContext(environment).Register(context);
        }
#endif

        // On utilise cet événement car il se déclenche après 'PostAcquireRequestState'
        // qui est utilisé par 'InitializeVSContextModule'.
        private void OnPreRequestHandlerExecute_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var environment = ChiffonContext.Resolve(app.Context).Environment;

            var language = environment.Language;

            if (!language.IsDefault())
            {
                InitializeCulture_(language.GetCultureInfo(), language.GetUICultureInfo());
            }
        }
    }
}