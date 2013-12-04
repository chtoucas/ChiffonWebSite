namespace Chiffon.Common
{
    using System.Diagnostics.CodeAnalysis;

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
    }
}