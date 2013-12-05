﻿namespace Chiffon.Infrastructure
{
    using System.Web;
    using Narvalo;

    public static class HttpContextExtensions
    {
        static string HttpContextKey_ = "ChiffonContext";

        public static void AddChiffonContext(this HttpContext @this, ChiffonContext context)
        {
            Requires.NotNull(@this, "this");

            @this.Items[HttpContextKey_] = context;
        }

        public static ChiffonContext GetChiffonContext(this HttpContext @this)
        {
            Requires.NotNull(@this, "this");

            var result = @this.Items[HttpContextKey_] as ChiffonContext;

            return result ?? new ChiffonContext();
        }
    }
}
