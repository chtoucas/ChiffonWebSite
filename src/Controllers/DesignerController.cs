namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Infrastructure.Addressing;

    [Authorize]
    public class DesignerController : PageController
    {
        public DesignerController(ISiteMapFactory sitemapFactory) : base(sitemapFactory) { }

        [HttpGet]
        public ActionResult Index(DesignerKey designer)
        {
            ViewBag.CanonicalLink = SiteMap.Designer(designer).ToString();

            return View(ViewName.Designer.Index);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designer, string reference)
        {
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designer, reference).ToString();

            return View(ViewName.Designer.Pattern);
        }
    }
}
