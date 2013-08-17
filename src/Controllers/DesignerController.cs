namespace Chiffon.Controllers
{
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
            var model = _queries.GetDesignerViewModel(designerKey, LanguageName);
            if (model == null) { return new HttpNotFoundResult(); }

            var patterns = _queries.ListPatterns(designerKey);
            model.Patterns = from _ in patterns
                              orderby _.CreationTime descending
                             select Mapper.Map(_, model.Designer.DisplayName);

            ViewBag.Title = SR.Designer_Index_Title;
            ViewBag.MetaDescription = SR.Designer_Index_Description;
            ViewBag.CanonicalLink = SiteMap.Designer(designerKey).ToString();

            return View(ViewName.Designer.Index, model);
        }

        [HttpGet]
        public ActionResult Category(DesignerKey designerKey, string categoryKey)
        {
            var model = _queries.GetDesignerViewModel(designerKey, LanguageName);
            if (model == null) { return new HttpNotFoundResult(); }

            var patterns = _queries.ListPatterns(designerKey, categoryKey);
            model.Patterns = from _ in patterns
                             orderby _.CreationTime descending
                             select Mapper.Map(_, model.Designer.DisplayName);

            ViewBag.Title = SR.Designer_Category_Title;
            ViewBag.MetaDescription = SR.Designer_Category_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerCategory(designerKey, categoryKey).ToString();

            return View(ViewName.Designer.Category, model);
        }

        [HttpGet]
        public ActionResult Pattern(DesignerKey designerKey, string categoryKey, string reference)
        {
            var model = _queries.GetDesignerViewModel(designerKey, LanguageName);
            if (model == null) { return new HttpNotFoundResult(); }

            var patterns = _queries.ListPatterns(designerKey, categoryKey);

            var pattern = from _ in patterns where _.Reference == reference select _;
            if (pattern.Count() == 0) { return new HttpNotFoundResult(); }

            patterns = from _ in patterns
                       orderby _.CreationTime descending
                       where _.Reference != reference
                       select _;

            model.Patterns = from _ in pattern.Concat(patterns)
                             select Mapper.Map(_, model.Designer.DisplayName);

            ViewBag.Title = SR.Designer_Pattern_Title;
            ViewBag.MetaDescription = SR.Designer_Pattern_Description;
            ViewBag.CanonicalLink = SiteMap.DesignerPattern(designerKey, categoryKey, reference).ToString();

            return View(ViewName.Designer.Category, model);
        }
    }
}
