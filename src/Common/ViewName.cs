namespace Chiffon.Common
{
    public static class ViewName
    {
        public static class Home
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
        }

        public static class Contact
        {
            public const string Login = "~/Views/Contact/Login.cshtml";
            public const string Register = "~/Views/Contact/Register.cshtml";
            public const string PostRegister = "~/Views/Contact/PostRegister.cshtml";
            public const string RegisterTwice = "~/Views/Contact/RegisterTwice.cshtml";
            public const string Newsletter = "~/Views/Contact/Newsletter.cshtml";
        }

        public static class Designer
        {
            public const string Index = "~/Views/Designer/Index.cshtml";
            public const string Category = "~/Views/Designer/Category.cshtml";
            public const string Pattern = "~/Views/Designer/Pattern.cshtml";
        }

        public static class Widget
        {
            public const string CommonJavaScript_Debug = "~/Views/Widget/JavaScript.Debug.cshtml";
            public const string CommonJavaScript_Release = "~/Views/Widget/JavaScript.Release.cshtml";
            public const string CommonStylesheet_Debug = "~/Views/Widget/Stylesheet.Debug.cshtml";
            public const string CommonStylesheet_Release = "~/Views/Widget/Stylesheet.Release.cshtml";
            //public const string GoogleAnalytics = "~/Views/Widget/GoogleAnalytics.cshtml";
            public const string LanguageMenu = "~/Views/Widget/LanguageMenu.cshtml";
        }

        public static class Shared
        {
            public const string ContactTitle = "~/Views/Shared/_ContactTitle.cshtml";
            public const string DesignerMenu = "~/Views/Shared/_DesignerMenu.cshtml";
            public const string DesignerInfo = "~/Views/Shared/_DesignerInfo.cshtml";
            public const string Html5Shim = "~/Views/Shared/_Html5Shim.cshtml";
            public const string PageTitle = "~/Views/Shared/_PageTitle.cshtml";
        }
    }
}