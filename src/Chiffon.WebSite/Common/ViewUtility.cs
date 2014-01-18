namespace Chiffon.Common
{
    using System;
    using Chiffon.Infrastructure;
    using Narvalo;

    public static class ViewUtility
    {
        public static string Localize(string viewName, ChiffonLanguage language)
        {
            Require.NotNull(viewName, "viewName");

            switch (language) {
                case ChiffonLanguage.English:
                    return viewName.Replace(".cshtml", ".en.cshtml");
                case ChiffonLanguage.Default:
                    return viewName;
                default:
                    throw new NotSupportedException("The requested language is not supported.");
            }
        }
    }
}