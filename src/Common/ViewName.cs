namespace Chiffon.Common
{
    public static class ViewName
    {
        // HomeController
        public static class Home
        {
            public const string
                Index = "~/Views/Home/Index.cshtml",
                About = "~/Views/Home/About.cshtml",
                Contact = "~/Views/Home/Contact.cshtml",
                Newsletter = "~/Views/Home/Newsletter.cshtml";
        }

        // AccountController.
        public static class Account
        {
            public const string
                Register = "~/Views/Account/Register.cshtml";
        }

        // MemberController.
        public static class Member
        {
            public const string
                Index = "~/Views/Member/Register.cshtml";
        }

        // Shared views.
        public static class Shared
        {
            public const string
                MemberMenu = "~/Views/Shared/_MemberMenu.cshtml",
                PatternPreviewsGrid = "~/Views/Shared/_PatternPreviewsGrid.cshtml";
        }
    }
}