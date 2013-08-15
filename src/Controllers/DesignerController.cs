namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Resources;

    [Authorize]
    public class DesignerController : PageController
    {
        [HttpGet]
        public ActionResult Index(DesignerKey designer)
        {
            ViewBag.Title = SR.Designer_Index_Title;
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designer).ToString();

            return View(ViewName.Designer.Index);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designer, string reference)
        {
            ViewBag.Title = SR.Designer_Pattern_Title;
            ViewBag.MetaDescription = SR.Designer_Pattern_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designer, reference).ToString();

            return View(ViewName.Designer.Pattern);
        }
    }
}
