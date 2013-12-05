namespace Chiffon.Infrastructure
{
    using System;
    using System.Threading;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    // TODO: Vérifier que ces événements ne sont déclenchés qu'une fois par requête.
    public class InitializeContextModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.BeginRequest += OnBeginRequest;
            context.PreRequestHandlerExecute += OnPreRequestHandlerExecute;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(InitializeContextModule));
        }

        void OnBeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;

            var environment = ChiffonEnvironmentResolver.Resolve(context.Request);

            context.AddChiffonContext(new ChiffonContext(environment));
        }

        // On utilise cet événement car il se déclenche après 'PostAcquireRequestState'
        // qui est utilisé par 'InitializeVSContextModule'.
        void OnPreRequestHandlerExecute(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var environment = app.Context.GetChiffonContext().Environment;

            if (!environment.Language.IsDefault()) {
                InitializeCulture_(environment.Culture);
            }
        }

        // WARNING: Cette méthode ne convient pas avec les actions asynchrones 
        // car on peut changer de Thread.
        static void InitializeCulture_(ChiffonCulture culture)
        {
            // Culture utilisée par ResourceManager.
            Thread.CurrentThread.CurrentUICulture = culture.UICulture;
            // Culture utilisée par System.Globalization.
            Thread.CurrentThread.CurrentCulture = culture.Culture;
        }
    }
}