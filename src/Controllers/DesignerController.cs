namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Controllers.Queries;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Narvalo;

    [Authorize]
    public class DesignerController : PageController
    {
        readonly DbHelper _dbHelper;

        public DesignerController(ChiffonEnvironment environment, ISiteMap siteMap, DbHelper dbHelper)
            : base(environment, siteMap)
        {
            Requires.NotNull(dbHelper, "dbHelper");

            _dbHelper = dbHelper;
        }

        [HttpGet]
        public ActionResult Index(DesignerKey designer)
        {
            var model = new GetDesignerQuery(_dbHelper).Execute(designer, LanguageName);

            ViewBag.Title = SR.Designer_Index_Title;
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designer).ToString();

            return View(ViewName.Designer.Index, model);
        }

        [HttpGet]
        public ActionResult Category(DesignerKey designer, string category)
        {
            var model = new GetCategoryQuery(_dbHelper).Execute(designer, category, LanguageName);

            ViewBag.Title = SR.Designer_Category_Title;
            ViewBag.MetaDescription = SR.Designer_Category_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerCategory(designer, category).ToString();

            return View(ViewName.Designer.Category, model);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designer, string reference)
        {
            var model = new GetPatternQuery(_dbHelper).Execute(designer, reference, LanguageName);
          
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
