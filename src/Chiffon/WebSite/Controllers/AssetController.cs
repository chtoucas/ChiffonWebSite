namespace Chiffon.WebSite.Controllers
{
    using System.Web.Mvc;

    public class AssetController : Controller
    {
        [ChildActionOnly]
        public PartialViewResult CommonJavaScript()
        {
            return PartialView("~/Views/Asset/Debug/JavaScript.cshtml");
        }

        [ChildActionOnly]
        public PartialViewResult CommonStylesheet()
        {
            return PartialView("~/Views/Asset/Debug/Stylesheet.cshtml");
        }
    }
}
