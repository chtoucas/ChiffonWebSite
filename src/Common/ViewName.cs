namespace Chiffon.Common
{
    public static class ViewName
    {
        public static class Home
        {
            public const string Index = "~/Views/Home/Index.cshtml";
            public const string About = "~/Views/Home/About.cshtml";
            public const string Contact = "~/Views/Home/Contact.cshtml";
            public const string Newsletter = "~/Views/Home/Newsletter.cshtml";
        }

        public static class Account
        {
            public const string Register = "~/Views/Account/Register.cshtml";
        }

        public static class Designer
        {
            public const string Index = "~/Views/Designer/Index.cshtml";
            public const string Pattern = "~/Views/Designer/Pattern.cshtml";
        }

        public static class Widget
        {
            public const string MemberMenu = "~/Views/Shared/_MemberMenu.cshtml";
            public const string PatternPreviewsGrid = "~/Views/Shared/_PatternPreviewsGrid.cshtml";
        }
    }
}