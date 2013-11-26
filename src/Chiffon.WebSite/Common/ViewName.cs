namespace Chiffon.Common
{
    public static class ViewName
    {
        public static class Home
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string AboutContent = "~/Views/Home/Resources/_AboutContent.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
            public const string ContactTitle = "~/Views/Home/Resources/_ContactTitle.cshtml";
        }

        public static class Account
        {
            public const string Login = "~/Views/Account/Login.cshtml";
            public const string LoginContent = "~/Views/Account/Resources/_LoginContent.cshtml";
            public const string PostRegisterEmail = "~/Views/Account/Resources/_PostRegisterEmail.cshtml";
            public const string Register = "~/Views/Account/Register.cshtml";
            public const string RegisterHelp = "~/Views/Account/Resources/_RegisterHelp.cshtml";
            public const string RegisterWarning = "~/Views/Account/Resources/_RegisterWarning.cshtml";
            public const string PostRegister = "~/Views/Account/PostRegister.cshtml";
            public const string RegisterTwice = "~/Views/Account/RegisterTwice.cshtml";
            public const string Newsletter = "~/Views/Account/Newsletter.cshtml";
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
            public const string GoogleAnalytics = "~/Views/Widget/GoogleAnalytics.cshtml";
            public const string LanguageMenu = "~/Views/Widget/LanguageMenu.cshtml";
        }

        public static class Shared
        {
            public const string DesignerMenu = "~/Views/Shared/_DesignerMenu.cshtml";
            public const string DesignerInfo = "~/Views/Shared/_DesignerInfo.cshtml";
            public const string Html5Shim = "~/Views/Shared/_Html5Shim.cshtml";
            public const string LayoutAuthorsRights = "~/Views/Shared/_LayoutAuthorsRights.cshtml";
            public const string LayoutTitle = "~/Views/Shared/_LayoutTitle.cshtml";
        }
    }
}