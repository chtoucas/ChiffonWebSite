namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;
    using Narvalo.Fx;

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

        // FIXME: La session peut ne pas être disponible à ce moment.
        // http://stackoverflow.com/questions/276355/can-i-access-session-state-from-an-httpmodule
        // http://forums.asp.net/p/1098574/1665773.aspx
        void OnPostAcquireRequestState(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;
            var context = app.Context;
            var session = app.Session;

            ChiffonEnvironmentResolver.MayResolve(context.Request, session)
                .WhenSome(_ => { context.AddChiffonContext(new ChiffonContext(_)); });
        }
    }
}