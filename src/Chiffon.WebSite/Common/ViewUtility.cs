namespace Chiffon.Common
{
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
                default:
                    return viewName;
            }
        }
    }
}