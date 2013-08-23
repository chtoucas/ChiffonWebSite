namespace Chiffon.Common
{
    public static class ActionName
    {
        // HomeController.
        public static class Home
        {
            public const string Index = "Index";
            public const string About = "About";
            public const string Contact = "Contact";
        }

        // ContactController.
        public static class Contact
        {
            public const string Register = "Register";
            public const string Login = "Login";
            public const string Newsletter = "Newsletter";
        }

        // DesignerController.
        public static class Designer
        {
            public const string Index = "Index";
            public const string Pattern = "Pattern";
            public const string Category = "Category";
        }

        // WidgetController.
        public static class Widget
        {
            public const string CommonJavaScript = "CommonJavaScript";
            public const string CommonStylesheet = "CommonStylesheet";
            public const string GoogleAnalytics = "GoogleAnalytics";
            public const string LanguageMenu = "LanguageMenu";
        }
    }
}