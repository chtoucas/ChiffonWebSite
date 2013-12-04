namespace Chiffon.Common
{
    using System.Diagnostics.CodeAnalysis;

    public static class ViewName
    {
        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Home
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string AboutContent = "~/Views/Home/Resources/_AboutContent.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
            public const string ContactTitle = "~/Views/Home/Resources/_ContactTitle.cshtml";
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Account
        {
            [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
            public const string Login = "~/Views/Account/Login.cshtml";
            [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
            public const string LoginContent = "~/Views/Account/Resources/_LoginContent.cshtml";
            public const string PostRegisterEmail = "~/Views/Account/Resources/_PostRegisterEmail.cshtml";
            public const string Register = "~/Views/Account/Register.cshtml";
            public const string RegisterHelp = "~/Views/Account/Resources/_RegisterHelp.cshtml";
            public const string RegisterWarning = "~/Views/Account/Resources/_RegisterWarning.cshtml";
            public const string PostRegister = "~/Views/Account/PostRegister.cshtml";
            public const string RegisterTwice = "~/Views/Account/RegisterTwice.cshtml";
            public const string Newsletter = "~/Views/Account/Newsletter.cshtml";
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Designer
        {
            public const string Index = "~/Views/Designer/Index.cshtml";
            public const string Category = "~/Views/Designer/Category.cshtml";
            public const string Pattern = "~/Views/Designer/Pattern.cshtml";
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
        public static class Widget
        {
            public const string CommonJavaScriptDebug = "~/Views/Widget/JavaScript.Debug.cshtml";
            public const string CommonJavaScriptRelease = "~/Views/Widget/JavaScript.Release.cshtml";
            public const string CommonStyleSheetDebug = "~/Views/Widget/Stylesheet.Debug.cshtml";
            public const string CommonStyleSheetRelease = "~/Views/Widget/Stylesheet.Release.cshtml";
            public const string GoogleAnalytics = "~/Views/Widget/GoogleAnalytics.cshtml";
            public const string LanguageMenu = "~/Views/Widget/LanguageMenu.cshtml";
        }

        [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
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