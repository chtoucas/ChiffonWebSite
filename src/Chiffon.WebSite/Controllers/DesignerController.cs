namespace Chiffon.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Chiffon.Common;
    using Chiffon.Data;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Chiffon.Resources;
    using Chiffon.Services;
    using Chiffon.ViewModels;
    using Narvalo;
    using Narvalo.Web.Semantic;
    using Addressing = Chiffon.Infrastructure.Addressing;

    [Authorize]
    public class DesignerController : ChiffonController
    {
        public const string AllCategoryKey = "ALL";

        // On limite le nombre d'aperçus à 30 = 5 * 6. En effet, suivant la taille de l'écran 
        // de l'internaute, on peut avoir de 2 à 3 colonnes, on choisit donc un multiple de 6.
        const int PreviewsPageSize_ = 18;

        readonly IQueries _queries;
        readonly IPatternService _patternService;

        public DesignerController(
            ChiffonEnvironment environment,
            Addressing.ISiteMap siteMap,
            IQueries queries,
            IPatternService patternService)
            : base(environment, siteMap)
        {
            Requires.NotNull(queries, "queries");
            Requires.NotNull(patternService, "patternService");

            _queries = queries;
            _patternService = patternService;
        }

        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
        public ActionResult Index(DesignerKey designerKey, int p = 1)
        {
            var pagedList = _patternService.ListPreviews(designerKey, p, PreviewsPageSize_);
            if (pagedList == null) { return new HttpNotFoundResult(); }

            var designer = GetDesignerViewItem_(designerKey);

            var model = new DesignerViewModel {
                Designer = designer,
                IsFirstPage = pagedList.IsFirstPage,
                IsLastPage = pagedList.IsLastPage,
                PageCount = pagedList.PageCount,
                PageIndex = pagedList.PageIndex,
                Previews = from _ in pagedList.Previews select ObjectMapper.Map(_, designer.DisplayName)
            };

            // Ontologie.
            Ontology.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Index_TitleFormat, model.Designer.DisplayName);
            Ontology.Description = SR.Designer_Index_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.Designer(designerKey, p);

            var image = model.Previews.First();
            SetOpenGraphImage_(designerKey, image.Reference, image.Variant);

            // ViewInfo.
            ViewInfo.AddAlternateUrls(Environment.Language, _ => _.Designer(designerKey, p));
            EnrichViewInfo_(designerKey);

            return View(Constants.ViewName.Designer.Index, model);
        }

        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
        public ActionResult Category(DesignerKey designerKey, string categoryKey, int p = 1)
        {
            var pagedList = _patternService.ListPreviews(designerKey, categoryKey, p, PreviewsPageSize_);
            if (pagedList == null) { return new HttpNotFoundResult(); }

            var designer = GetDesignerViewItem_(designerKey);
            var category = (from _ in designer.Categories where _.Key == categoryKey select _).Single();

            var model = new CategoryViewModel {
                Category = category,
                Designer = designer,
                IsFirstPage = pagedList.IsFirstPage,
                IsLastPage = pagedList.IsLastPage,
                PageCount = pagedList.PageCount,
                PageIndex = pagedList.PageIndex,
                Previews = from _ in pagedList.Previews select ObjectMapper.Map(_, designer.DisplayName)
            };

            // Ontologie.
            Ontology.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Category_TitleFormat,
                model.Category.DisplayName, model.Designer.DisplayName);
            Ontology.Description = SR.Designer_Category_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.DesignerCategory(designerKey, categoryKey, p);

            var image = model.Previews.First();
            SetOpenGraphImage_(designerKey, image.Reference, image.Variant);

            // ViewInfo.
            ViewInfo.AddAlternateUrls(Environment.Language, _ => _.DesignerCategory(designerKey, categoryKey, p));
            EnrichViewInfo_(designerKey, categoryKey);

            return View(Constants.ViewName.Designer.Category, model);
        }

        [HttpGet]
        [SuppressMessage("Microsoft.Design", "CA1026:DefaultParametersShouldNotBeUsed")]
        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "p")]
        public ActionResult Pattern(DesignerKey designerKey, string categoryKey, string reference, int p = 1)
        {
            //var pagedList = _patternService.ListPreviews(designerKey, categoryKey, p, PreviewsPageSize_);
            //if (pagedList == null) { return new HttpNotFoundResult(); }

            var views = _patternService.GetPatternViews(designerKey, categoryKey, reference);
            if (views.Count() == 0) { return new HttpNotFoundResult(); }

            var designer = GetDesignerViewItem_(designerKey);
            var category = (from _ in designer.Categories where _.Key == categoryKey select _).Single();

            var model = new PatternViewModel {
                Category = category,
                Designer = designer,
                PatternViews = from _ in views select ObjectMapper.Map(_, designer.DisplayName),
                Reference = reference,
                //IsFirstPage = pagedList.IsFirstPage,
                //IsLastPage = pagedList.IsLastPage,
                //PageCount = pagedList.PageCount,
                //PageIndex = pagedList.PageIndex,
                //Previews = from _ in pagedList.Previews select ObjectMapper.Map(_, designer.DisplayName)
            };


            // Ontologie.
            Ontology.Title = String.Format(
                CultureInfo.CurrentUICulture, SR.Designer_Pattern_TitleFormat,
                reference, model.Designer.DisplayName);
            Ontology.Description = SR.Designer_Pattern_Description;
            Ontology.Relationships.CanonicalUrl = SiteMap.DesignerPattern(designerKey, categoryKey, reference, p);

            var image = views.First();
            SetOpenGraphImage_(designerKey, image.Reference, image.Variant);

            // ViewInfo.
            ViewInfo.AddAlternateUrls(Environment.Language, _ => _.DesignerPattern(designerKey, categoryKey, reference, p));
            EnrichViewInfo_(designerKey, categoryKey);

            return View(Constants.ViewName.Designer.Pattern, model);
        }

        #region Utilitaires.

        // On suppose que designerKey est toujours valide (une contrainte sur la route doit
        // assurer qu'on se retrouve dans cette configuration).

        void EnrichViewInfo_(DesignerKey designerKey, string categoryKey = AllCategoryKey)
        {
            ViewInfo.DesignerClass = CssUtility.DesignerClass(designerKey);
            ViewInfo.CurrentCategoryKey = categoryKey;
        }

        DesignerViewItem GetDesignerViewItem_(DesignerKey designerKey)
        {
            var designer = _queries.GetDesigner(designerKey, UICulture);
            var categories = _queries.ListCategories(designerKey);

            return ObjectMapper.Map(designer, categories);
        }

        void SetOpenGraphImage_(DesignerKey designerKey, string reference, string variant)
        {
            var imageUrl = new Uri(Url.PreviewContent(designerKey, reference, variant, true /* absolute */));
            Ontology.OpenGraph.Image = new OpenGraphImage(imageUrl) {
                Height = ImageGeometry.PreviewHeight,
                MimeType = OpenGraphImage.JpegMimeType,
                Width = ImageGeometry.PreviewWidth,
            };
        }

        #endregion
    }
}
