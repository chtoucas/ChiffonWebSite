namespace Chiffon.Common
{
    using System.Diagnostics.CodeAnalysis;

    public static class Constants
    {
        public static class ActionName
        {
            // HomeController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Home
            {
                public const string Index = "Index";
                public const string About = "About";
                public const string Contact = "Contact";
            }

            // ContactController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Contact
            {
                public const string Register = "Register";
                [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
                public const string Login = "Login";
                public const string Newsletter = "Newsletter";
            }

            // DesignerController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Designer
            {
                public const string Index = "Index";
                public const string Pattern = "Pattern";
                public const string Category = "Category";
            }

            // WidgetController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Widget
            {
                public const string CommonJavaScript = "CommonJavaScript";
                public const string CommonStyleSheet = "CommonStylesheet";
                public const string GoogleAnalytics = "GoogleAnalytics";
                public const string LanguageMenu = "LanguageMenu";
            }

            // MailController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Mail
            {
                public const string ForgottenPassword = "ForgottenPassword";
                public const string Welcome = "Welcome";
            }
        }

        public static class ControllerName
        {
            public const string Account = "Account";
            public const string Designer = "Designer";
            public const string Home = "Home";
            public const string Widget = "Widget";
            public const string Mail = "Mail";
        }

        public static class CssClassName
        {
            public const string EstherMarthi = "em";
            public const string VivianeDevaux = "vd";
            public const string LaureRoussel = "lr";
            public const string ChristineLégeret = "cl";
        }

        public static class RouteName
        {
            // HomeController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Home
            {
                public const string About = "Home/About";
                public const string Contact = "Home/Contact";
                public const string Index = "Home/";
            }

            // AccountController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Account
            {
                [SuppressMessage("Microsoft.Naming", "CA1726:UsePreferredTerms", MessageId = "Login")]
                public const string Login = "Contact/Login";
                public const string Newsletter = "Contact/Newsletter";
                public const string Register = "Contact/Register";
            }

            // DesignerController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Designer
            {
                public const string Category = "Designer/Category";
                public const string Index = "Designer/Index";
                public const string Pattern = "Designer/Pattern";
            }

            // DesignerController.
            [SuppressMessage("Microsoft.Design", "CA1034:NestedTypesShouldNotBeVisible")]
            public static class Mail
            {
                public const string ForgottenPassword = "Mail/ForgottenPassword";
                public const string Welcome = "Mail/Welcome";
            }
        }

        public static class ViewName
        {
            public const string DesignerMenu = "~/Views/Shared/_DesignerMenu.cshtml";
            public const string DesignerInfo = "~/Views/Shared/_DesignerInfo.cshtml";
            public const string Html5Shim = "~/Views/Shared/_Html5Shim.cshtml";
            public const string LayoutAuthorsRights = "~/Views/Shared/_LayoutAuthorsRights.cshtml";
            public const string LayoutCopyright = "~/Views/Shared/_LayoutCopyright.cshtml";
            public const string LayoutTitle = "~/Views/Shared/_LayoutTitle.cshtml";

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
            public static class Mail
            {
                public const string ForgottenPassword = "~/Views/Mail/ForgottenPassword.cshtml";
                public const string Welcome = "~/Views/Mail/Welcome.cshtml";
            }
        }
    }
}