﻿namespace Chiffon.Infrastructure
{
    using System;
    using System.Web;
    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using Narvalo;

    public class HttpHeaderPolicyModule : IHttpModule
    {
        #region IHttpModule

        public void Init(HttpApplication context)
        {
            Requires.NotNull(context, "context");

            context.PreSendRequestHeaders += OnPreSendRequestHeaders;
        }

        public void Dispose()
        {
            ;
        }

        #endregion

        public static void Register()
        {
            DynamicModuleUtility.RegisterModule(typeof(HttpHeaderPolicyModule));
        }

        void OnPreSendRequestHeaders(object sender, EventArgs e)
        {
            var app = sender as HttpApplication;

            var response = app.Response;
            if (response == null) {
                // Peut arriver si trySkipIisCustomErrors est égal à true.
                return;
            }

            // Habituellement on essaie de prévenir l'inclusion d'une page du site dans une "frame" via javascript.
            // Malheureusement cela ne suffit pas et il existe des techniques pour contrer les techniques basées sur javascript.
            // Cf. http://en.wikipedia.org/wiki/Framekiller
            response.Headers.Add("X-Frame-Options", "DENY");
        }
    }
}