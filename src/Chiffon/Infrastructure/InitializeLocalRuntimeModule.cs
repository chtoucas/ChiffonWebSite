namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    public class InitializeLocalRuntimeModule : IHttpModule
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
            DynamicModuleUtility.RegisterModule(typeof(InitializeLocalRuntimeModule));
        }

        void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            ChiffonRuntime.Initialize(app.Request, app.Session);
        }
    }
}