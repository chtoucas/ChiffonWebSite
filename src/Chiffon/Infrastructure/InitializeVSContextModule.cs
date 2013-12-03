namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    public class InitializeVSContextModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.PostAcquireRequestState += OnPostAcquireRequestState;
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

        void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;
            var session = app.Session;

            var environment = ChiffonEnvironmentResolver.Resolve(context.Request, session);

            if (environment != null) {
                ChiffonContext.Initialize(new ChiffonContext(environment), context);
            }
        }
    }
}