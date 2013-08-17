﻿namespace Chiffon.Controllers
{
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Data;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Fx;

    [Authorize]
    public class DesignerController : PageController
    {
        readonly DataContext _dataContext;

        public DesignerController(ChiffonEnvironment environment, ISiteMap siteMap, DataContext dataContext)
            : base(environment, siteMap)
        {
            Requires.NotNull(dataContext, "dataContext");

            _dataContext = dataContext;
        }

        [HttpGet]
        public ActionResult Index(DesignerKey designerKey)
        {
            DesignerViewModel model = _dataContext.GetDesignerView(designerKey, LanguageName);

            ViewBag.Title = SR.Designer_Index_Title;
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designerKey).ToString();

            return View(ViewName.Designer.Index, model);
        }

        [HttpGet]
        public ActionResult Category(DesignerKey designerKey, string categoryKey)
        {
            Maybe<CategoryViewModel> model
                = _dataContext.MayGetCategoryView(designerKey, categoryKey, LanguageName);

            if (model.IsNone) {
                return new HttpNotFoundResult();
            }

            ViewBag.Title = SR.Designer_Category_Title;
            ViewBag.MetaDescription = SR.Designer_Category_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerCategory(designerKey, categoryKey).ToString();

            return View(ViewName.Designer.Category, model.Value);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designerKey, string categoryKey, string reference)
        {
            Maybe<CategoryViewModel> model
                = _dataContext.MayGetPatternView(designerKey, categoryKey, reference, LanguageName);

            if (model.IsNone) {
                return new HttpNotFoundResult();
            }

            ViewBag.Title = SR.Designer_Pattern_Title;
            ViewBag.MetaDescription = SR.Designer_Pattern_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designerKey, categoryKey, reference).ToString();

            return View(ViewName.Designer.Category, model.Value);
        }
    }
}
