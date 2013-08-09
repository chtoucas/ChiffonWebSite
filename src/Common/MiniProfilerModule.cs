namespace Chiffon.WebSite
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;
    using StackExchange.Profiling;

    internal class MiniProfilerModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.BeginRequest += OnBeginRequest_;
            //context.AuthenticateRequest += OnAuthenticateRequest;
            context.EndRequest += OnEndRequest_;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void SelfRegister()
        {
            DynamicModuleUtility.RegisterModule(typeof(MiniProfilerModule));
        }

        void OnBeginRequest_(object sender, EventArgs e)
        {
            var request = ((HttpApplication)sender).Request;
            //TODO: By default only local requests are profiled, optionally you can set it up
            //  so authenticated users are always profiled
            if (request.IsLocal) { MiniProfiler.Start(); }
        }

        //private void OnAuthenticateRequest(object sender, EventArgs e)
        //{
        //    // TODO: You can control who sees the profiling information
        //    if (!CurrentUserIsAllowedToSeeProfiler()) {
        //        StackExchange.Profiling.MiniProfiler.Stop(discardResults: true);
        //    }
        //}

        void OnEndRequest_(object sender, EventArgs e)
        {
            MiniProfiler.Stop();
        }
    }
}
