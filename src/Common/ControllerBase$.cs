namespace Chiffon.Common
{
    using System.Web.Mvc;

    public static class ControllerBaseExtensions
    {
        public static void SetTitle(this ControllerBase self, string title)
        {
            self.ViewBag.Title = title;
        }

        public static void SetMetaDescription(this ControllerBase self, string description)
        {
            self.ViewBag.MetaDescription = description;
        }

        public static void SetMetaKeywords(this ControllerBase self, string keywords)
        {
            self.ViewBag.MetaKeywords = keywords;
        }
    }
}
