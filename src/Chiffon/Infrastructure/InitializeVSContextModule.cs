namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using System.Web.SessionState;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    public class InitializeVSContextModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Require.NotNull(context, "context");

            context.PostAcquireRequestState += OnPostAcquireRequestState_;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(InitializeVSContextModule));
        }

        // NB: La session peut ne pas être disponible à ce moment.
        // http://stackoverflow.com/questions/276355/can-i-access-session-state-from-an-httpmodule
        void OnPostAcquireRequestState_(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;
            var request = context.Request;

            if (!request.IsLocal) {
                throw new InvalidOperationException(
                    "This module is only available when run locally.");
            }

            if (!(context.Handler is IRequiresSessionState)) {
                // NB: Normalement, on ne devrait pas avoir à faire cette vérification,
                // mais cela peut poser des problèmes si il s'agit d'une requête prise
                // en charge par IIS en cas d'erreur (cf. httpErrors et customErrors dans
                // web.config).
                return;
            }

            var env = ChiffonEnvironmentResolver.Resolve(request, app.Session);

            context.AddChiffonContext(new ChiffonContext(env));
        }
    }
}