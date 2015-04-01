namespace Chiffon.Views
{
    using System;

    using Chiffon.Common;
    using Chiffon.Entities;
    using Narvalo;

    public static class ViewUtility
    {
        public static string DesignerClass(DesignerKey key)
        {
            if (key == DesignerKey.ChristineLégeret)
            {
                return Constants.CssClassName.ChristineLégeret;
            }
            else if (key == DesignerKey.EstherMarthi)
            {
                return Constants.CssClassName.EstherMarthi;
            }
            else if (key == DesignerKey.LaureRoussel)
            {
                return Constants.CssClassName.LaureRoussel;
            }
            else if (key == DesignerKey.VivianeDevaux)
            {
                return Constants.CssClassName.VivianeDevaux;
            }
            else
            {
                throw new NotSupportedException();
            }
        }

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