namespace Chiffon
{
    public static class Constants
    {
        public const string ContactAddress = "contact@vivianedevaux.org";

        public static class ActionName
        {
            // AccountController.
            public static class Account
            {
                public const string Login = "Login";
                public const string Register = "Register";
                public const string RegisterSuccess = "RegisterSuccess";
            }

            // ComponentController.
            public static class Component
            {
                public const string CommonJavaScript = "CommonJavaScript";
                public const string LanguageMenu = "LanguageMenu";
            }

            // DesignerController.
            public static class Designer
            {
                public const string Index = "Index";
                public const string Pattern = "Pattern";
                public const string Category = "Category";
            }

            // HomeController.
            public static class Home
            {
                public const string Index = "Index";
                public const string About = "About";
                public const string Contact = "Contact";
                public const string ContactSuccess = "ContactSuccess";
            }

            // WidgetController.
            public static class Widget
            {
                public const string AuthorsRights = "AuthorsRights";
                public const string CommonStyleSheet = "CommonStylesheet";
                public const string Copyright = "Copyright";
                public const string GoogleAnalytics = "GoogleAnalytics";
                public const string Html5Shim = "Html5Shim";
                public const string Title = "Title";
            }
        }

        public static class ControllerName
        {
            public const string Account = "Account";
            public const string Component = "Component";
            public const string Designer = "Designer";
            public const string Home = "Home";
            public const string Widget = "Widget";
        }

        public static class CssClassName
        {
            public const string EstherMarthi = "em";
            public const string VivianeDevaux = "vd";
            public const string LaureRoussel = "lr";
            public const string ChristineLégeret = "cl";
        }

        public static class ImageGeometry
        {
            public const int PatternWidth = 700;
            public const int PatternHeight = 520;
            public const int PreviewWidth = 220;
            public const int PreviewHeight = 160;
        }

        public static class RouteName
        {
            // AccountController.
            public static class Account
            {
                public const string Login = "Account/Login";
                public const string Register = "Account/Register";
                public const string RegisterSuccess = "Account/RegisterSuccess";
            }

            // DesignerController.
            public static class Designer
            {
                public const string Category = "Designer/Category";
                public const string Index = "Designer/Index";
                public const string Pattern = "Designer/Pattern";
            }

            // HomeController.
            public static class Home
            {
                public const string About = "Home/About";
                public const string Contact = "Home/Contact";
                public const string ContactSuccess = "Home/ContactConfimation";
                public const string Index = "Home/";
            }
        }

        public static class RoutePath
        {
            public const string About = "informations";
            public const string Contact = "contact";
            public const string Login = "connexion";
            public const string Register = "inscription";

            // TODO: Utiliser ces constantes dans SiteMap (cf. SmartFormat.NET).
            public const string Designer = "{designerKey}";
            public const string DesignerCategory = "{designerKey}/{categoryKey}";
            public const string DesignerPattern = "{designerKey}/{categoryKey}/{reference}";

            public const string Go = "go";
            public const string LogOff = "disconnecte";
            public const string LogOn = "connecte";
            public const string Pattern = "motif";
        }

        public static class SiteMap
        {
            public const string PageKey = "p";
            public const string ReturnUrl = "returnUrl";
            public const string TargetUrl = "targetUrl";
        }

        public static class ViewName
        {
            public const string DesignerInfo = "~/Views/Shared/_DesignerInfo.cshtml";
            public const string UserMenu = "~/Views/Shared/_UserMenu.cshtml";

            public static class Account
            {
                public const string Login = "~/Views/Account/Login.cshtml";
                public const string Register = "~/Views/Account/Register.cshtml";
                public const string RegisterSuccess = "~/Views/Account/RegisterSuccess.cshtml";
                public const string RegisterFailure = "~/Views/Account/RegisterFailure.cshtml";
            }

            public static class Component
            {
                public const string CommonJavaScriptDebug = "~/Views/Component/JavaScript.Debug.cshtml";
                public const string CommonJavaScriptRelease = "~/Views/Component/JavaScript.Release.cshtml";
                public const string LanguageMenu = "~/Views/Component/LanguageMenu.cshtml";
            }

            public static class Designer
            {
                public const string Index = "~/Views/Designer/Index.cshtml";
                public const string Category = "~/Views/Designer/Category.cshtml";
                public const string Pattern = "~/Views/Designer/Pattern.cshtml";
            }

            public static class Home
            {
                public const string Index = "~/Views/Home/Index.cshtml";
                public const string About = "~/Views/Home/About.cshtml";
                public const string AboutContent = "~/Views/Home/Resources/_AboutContent.cshtml";
                public const string Contact = "~/Views/Home/Contact.cshtml";
                public const string ContactSuccess = "~/Views/Home/ContactSuccess.cshtml";
            }

            public static class Widget
            {
                public const string AuthorsRights = "~/Views/Widget/AuthorsRights.cshtml";
                public const string CommonStyleSheetDebug = "~/Views/Widget/Stylesheet.Debug.cshtml";
                public const string CommonStyleSheetRelease = "~/Views/Widget/Stylesheet.Release.cshtml";
                public const string Copyright = "~/Views/Widget/Copyright.cshtml";
                public const string GoogleAnalytics = "~/Views/Widget/GoogleAnalytics.cshtml";
                public const string Html5Shim = "~/Views/Widget/Html5Shim.cshtml";
                public const string Title = "~/Views/Widget/Title.cshtml";
            }
        }
    }
}