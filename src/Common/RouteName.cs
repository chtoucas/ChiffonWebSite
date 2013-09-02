namespace Chiffon.Common
{
    public static class RouteName
    {
        // HomeController.
        public static class Home
        {
            public const string About = "Home/About";
            public const string Contact = "Home/Contact";
            public const string Index = "Home/";
        }

        // ContactController.
        public static class Contact
        {
            public const string Login = "Contact/Login";
            public const string Newsletter = "Contact/Newsletter";
            public const string Register = "Contact/Register";
        }

        // DesignerController.
        public static class Designer
        {
            public const string Category = "Designer/Category";
            public const string Index = "Designer/Index";
            public const string Pattern = "Designer/Pattern";
        }
    }
}