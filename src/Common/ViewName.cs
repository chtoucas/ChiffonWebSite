namespace Chiffon.Common
{
    public static class ViewName
    {
        public static class Home
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string AboutEnglish = "~/Views/Home/About.en.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
            public const string Newsletter = "~/Views/Home/Newsletter.cshtml";
        }

        public static class Account
        {
            public const string Login = "~/Views/Account/Login.cshtml";
            public const string Register = "~/Views/Account/Register.cshtml";
        }

        public static class Designer
        {
            public const string Index = "~/Views/Designer/Index.cshtml";
            public const string Category = "~/Views/Designer/Category.cshtml";
        }

        public static class Widget
        {
            public const string CommonJavaScript_Debug = "~/Views/Widget/JavaScript.Debug.cshtml";
            public const string CommonJavaScript_Release = "~/Views/Widget/JavaScript.Release.cshtml";
            public const string CommonStylesheet_Debug = "~/Views/Widget/Stylesheet.Debug.cshtml";
            public const string CommonStylesheet_Release = "~/Views/Widget/Stylesheet.Release.cshtml";
        }

        public static class Shared
        {
            public const string DesignerMenu = "~/Views/Shared/_DesignerMenu.cshtml";
            public const string DesignerInfo = "~/Views/Shared/_DesignerInfo.cshtml";
            public const string PageTitle = "~/Views/Shared/_Title.cshtml";
            public const string PageTitleEnglish = "~/Views/Shared/_Title.en.cshtml";
        }
    }
}