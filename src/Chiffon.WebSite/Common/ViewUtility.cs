namespace Chiffon.Common
{
    using System;
    using System.Security.Principal;
    using Chiffon.Infrastructure;
    using Narvalo;

    public static class ViewUtility
    {
        public static string Localize(string viewName, ChiffonLanguage language)
        {
            Requires.NotNull(viewName, "viewName");

            switch (language) {
                case ChiffonLanguage.English:
                    return viewName.Replace(".cshtml", ".en.cshtml");
                case ChiffonLanguage.Default:
                    return viewName;
                default:
                    throw new NotSupportedException("The requested language is not supported.");
            }
        }

        public static string NofollowAttribute(IPrincipal user)
        {
            return user.Identity.IsAuthenticated ? String.Empty : " rel=nofollow";
        }
    }
}