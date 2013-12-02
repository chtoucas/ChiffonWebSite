namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    // FIXME: Supprimer l'instruction #if, Pour le moment, je ne vois pas une autre manière simple de faire.
    public class InitializeRuntimeModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

#if RELEASE
            context.BeginRequest += OnBeginRequest;
#else
            context.PostAcquireRequestState += OnPostAcquireRequestState;
#endif
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(InitializeRuntimeModule));
        }

#if RELEASE
        void OnBeginRequest(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            ChiffonRuntime.Initialize(app.Request);
        }
#else
        void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            ChiffonRuntime.Initialize(app.Request, app.Session);
        }
#endif
    }
}