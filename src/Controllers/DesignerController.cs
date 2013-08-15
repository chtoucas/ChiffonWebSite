namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Entities;
    using Chiffon.Resources;
    using Narvalo;

    [Authorize]
    public class DesignerController : PageController
    {
        readonly ViewModelStore _store;

        public DesignerController(ViewModelStore store)
        {
            Requires.NotNull(store, "store");

            _store = store;
        }

        [HttpGet]
        public ActionResult Index(DesignerKey designer)
        {
            var model = _store.Designer(designer, Language);

            ViewBag.Title = SR.Designer_Index_Title;
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designer).ToString();

            return View(ViewName.Designer.Index, model);
        }

        [HttpGet]
        public ActionResult Category(DesignerKey designer, string category)
        {
            var model = _store.Category(designer, category, Language);

            ViewBag.Title = SR.Designer_Category_Title;
            ViewBag.MetaDescription = SR.Designer_Category_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerCategory(designer, category).ToString();

            return View(ViewName.Designer.Category, model);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designer, string reference)
        {
            var model = _store.Pattern(designer, reference, Language);
          
            if (model.Pattern == null) {
                return new HttpNotFoundResult("XXX");
            }

            ViewBag.Title = SR.Designer_Pattern_Title;
            ViewBag.MetaDescription = SR.Designer_Pattern_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designer, reference).ToString();

            return View(ViewName.Designer.Pattern, model);
        }
    }
}
