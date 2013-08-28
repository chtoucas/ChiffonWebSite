namespace Chiffon.Controllers
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Data;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Infrastructure.Addressing;
    using Chiffon.Resources;
    using Chiffon.ViewModels;
    using Narvalo;

    [Authorize]
    public class DesignerController : PageController
    {
        readonly IQueries _queries;

        public DesignerController(ChiffonEnvironment environment, ISiteMap siteMap, IQueries queries)
            : base(environment, siteMap)
        {
            Requires.NotNull(queries, "queries");

            _queries = queries;
        }

        DesignerViewModel GetBaseModel_(DesignerKey designerKey)
        {
            var designer = _queries.GetDesigner(designerKey, Culture);
            if (designer == null) { return null; }
            var categories = _queries.ListCategories(designerKey);

            return new DesignerViewModel {
                Categories = from _ in categories select Mapper.Map(_),
                Designer = Mapper.Map(designer),
            };
        }

        [HttpGet]
        public ActionResult Index(DesignerKey designerKey)
        {
            var model = GetBaseModel_(designerKey);
            var patterns = _queries.ListPatterns(designerKey);

            model.Patterns = from _ in patterns
                             where _.HasPreview
                             orderby _.LastModifiedTime descending
                             select Mapper.Map(_, model.Designer.DisplayName);

            ViewBag.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewBag.CurrentCategoryKey = "ALL";

            ViewBag.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Index_TitleFormat, model.Designer.DisplayName);
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designerKey).ToString();

            return View(ViewName.Designer.Index, model);
        }

        [HttpGet]
        public ActionResult Category(DesignerKey designerKey, string categoryKey)
        {
            var model = GetBaseModel_(designerKey);
            var patterns = _queries.ListPatterns(designerKey, categoryKey);

            model.Patterns = from _ in patterns
                             orderby _.LastModifiedTime descending
                             select Mapper.Map(_, model.Designer.DisplayName);

            var category = (from _ in model.Categories where _.Key == categoryKey select _).Single();

            ViewBag.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewBag.CurrentCategoryKey = categoryKey;

            ViewBag.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Category_TitleFormat,
                category.DisplayName, model.Designer.DisplayName);
            ViewBag.MetaDescription = SR.Designer_Category_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerCategory(designerKey, categoryKey).ToString();

            return View(ViewName.Designer.Category, model);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designerKey, string categoryKey, string reference)
        {
            var designer = _queries.GetDesigner(designerKey, Culture);
            if (designer == null) { return new HttpNotFoundResult(); }
            var categories = _queries.ListCategories(designerKey);
            var patterns = _queries.ListPatterns(designerKey, categoryKey);

            var pattern = (from _ in patterns where _.Reference == reference select _).FirstOrDefault();
            //if (pattern.Count() == 0) { return new HttpNotFoundResult(); }
            if (pattern == null) { return new HttpNotFoundResult(); }

            patterns = from _ in patterns
                       orderby _.LastModifiedTime descending
                       where _.Reference != reference && _.Preferred
                       select _;

            var model = new DesignerViewModel {
                Categories = from _ in categories select Mapper.Map(_),
                Designer = Mapper.Map(designer),
                Patterns = from _ in patterns.Prepend(pattern) select Mapper.Map(_, designer.DisplayName)
            };

            ViewBag.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewBag.CurrentCategoryKey = categoryKey;

            ViewBag.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Pattern_TitleFormat,
                pattern.Reference, designer.DisplayName);
            ViewBag.MetaDescription = SR.Designer_Pattern_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designerKey, categoryKey, reference).ToString();

            return View(ViewName.Designer.Category, model);
        }
    }
}
