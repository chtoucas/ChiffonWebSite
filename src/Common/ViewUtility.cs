namespace Chiffon.Common
{
    using Chiffon.Infrastructure;

    public static class ViewUtility
    {
        public static string Localize(string viewName, ChiffonLanguage language)
        {
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