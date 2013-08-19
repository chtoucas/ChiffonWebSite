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

        [HttpGet]
        public ActionResult Index(DesignerKey designerKey)
        {
            var designer = _queries.GetDesigner(designerKey, Culture);
            if (designer == null) { return new HttpNotFoundResult(); }
            var categories = _queries.ListCategories(designerKey);
            var patterns = _queries.ListPatterns(designerKey);

            var model = new DesignerViewModel {
                Categories = from _ in categories select Mapper.Map(_),
                Designer = Mapper.Map(designer),
                Patterns = from _ in patterns
                           orderby _.CreationTime descending
                           select Mapper.Map(_, designer.DisplayName)
            };

            ViewBag.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewBag.CurrentCategoryKey = "ALL";

            ViewBag.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Index_TitleFormat, designer.DisplayName);
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designerKey).ToString();

            return View(ViewName.Designer.Index, model);
        }

        [HttpGet]
        public ActionResult Category(DesignerKey designerKey, string categoryKey)
        {
            var designer = _queries.GetDesigner(designerKey, Culture);
            if (designer == null) { return new HttpNotFoundResult(); }
            var categories = _queries.ListCategories(designerKey);
            var patterns = _queries.ListPatterns(designerKey, categoryKey);

            var model = new DesignerViewModel {
                Categories = from _ in categories select Mapper.Map(_),
                Designer = Mapper.Map(designer),
                Patterns = from _ in patterns
                           orderby _.CreationTime descending
                           select Mapper.Map(_, designer.DisplayName)
            };

            var category = (from _ in categories where _.Key == categoryKey select _).Single();

            ViewBag.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewBag.CurrentCategoryKey = categoryKey;

            ViewBag.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Category_TitleFormat,
                category.DisplayName, designer.DisplayName);
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

            var pattern = from _ in patterns where _.Reference == reference select _;
            if (pattern.Count() == 0) { return new HttpNotFoundResult(); }

            patterns = from _ in patterns
                       orderby _.CreationTime descending
                       where _.Reference != reference
                       select _;

            var model = new DesignerViewModel {
                Categories = from _ in categories select Mapper.Map(_),
                Designer = Mapper.Map(designer),
                Patterns = from _ in pattern.Concat(patterns) select Mapper.Map(_, designer.DisplayName)
            };

            ViewBag.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewBag.CurrentCategoryKey = categoryKey;

            ViewBag.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Pattern_TitleFormat, 
                pattern.Single().Reference, designer.DisplayName);
            ViewBag.MetaDescription = SR.Designer_Pattern_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designerKey, categoryKey, reference).ToString();

            return View(ViewName.Designer.Category, model);
        }
    }
}
