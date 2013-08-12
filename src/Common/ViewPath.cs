namespace Chiffon.Common
{
    public static class ViewPath
    {
        // HomeController
        public static class Home
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
            public const string Newsletter = "~/Views/Home/Newsletter.cshtml";
        }

        // AccountController.
        public static class Account
        {
            public const string Register = "~/Views/Account/Register.cshtml";
        }

        // DesignerController.
        public static class Designer
        {
            public const string Index = "~/Views/Member/Index.cshtml";
            public const string Pattern = "~/Views/Member/Pattern.cshtml";
        }

        // Shared views.
        public static class Shared
        {
            public const string MemberMenu = "~/Views/Shared/_MemberMenu.cshtml";
            public const string PatternPreviewsGrid = "~/Views/Shared/_PatternPreviewsGrid.cshtml";
        }
    }
}