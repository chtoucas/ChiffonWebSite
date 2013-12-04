namespace Chiffon.Common
{
    using System.Diagnostics.CodeAnalysis;

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
    }
}