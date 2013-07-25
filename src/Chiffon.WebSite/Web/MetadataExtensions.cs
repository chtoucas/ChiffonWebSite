namespace Narvalo.Web.Html {
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Text;
    using System.Web.Mvc;

    public static class MetadataExtensions {
        //public static MvcHtmlString WatermarksForModel(this HtmlHelper html) {
        //    ModelMetadata metadata = ModelMetadata.FromStringExpression(string.Empty, html.ViewContext.ViewData);
        //    var watermarks = metadata.Properties.Where(
        //        m => !string.IsNullOrEmpty(m.Watermark))
        //    .Select(m => new {
        //        PropertyName = m.PropertyName,
        //        Watermark = m.Watermark.Replace(@"'", @"\'")
        //    });

        //    StringBuilder sb = new StringBuilder();

        //    foreach (var watermark in watermarks) {
        //        sb.AppendFormat("$('#{0}').watermark('{1}');", watermark.PropertyName, watermark.Watermark);
        //    }

        //    return MvcHtmlString.Create(sb.ToString());
        //}

        public static MvcHtmlString EditMasksForModel(this HtmlHelper html) {
            ModelMetadata metadata = ModelMetadata.FromStringExpression(string.Empty, html.ViewContext.ViewData);
            var masks = metadata.Properties.Where(
                m => m.AdditionalValues.ContainsKey("EditMask"))
            .Select(m => new {
                PropertyName = m.PropertyName,
                EditMask = (m.AdditionalValues["EditMask"] as string).Replace(@"'", @"\'")
            });

            StringBuilder sb = new StringBuilder();

            foreach (var mask in masks) {
                sb.AppendFormat("$('#{0}').mask('{1}');", mask.PropertyName, mask.EditMask);
            }

            return MvcHtmlString.Create(sb.ToString());
        }

        //public static MvcHtmlString WatermarkForModel<TModel, TProperty>(
        //    this HtmlHelper<TModel> html,
        //    Expression<Func<TModel, TProperty>> expr) {

        //    var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expr, html.ViewData);

        //    return MvcHtmlString.Create(metadata.Watermark);
        //}

        //public static MvcHtmlString EditMaskForModel<TModel, TProperty>(
        //    this HtmlHelper<TModel> html,
        //    Expression<Func<TModel, TProperty>> expr) {

        //    var metadata = ModelMetadata.FromLambdaExpression<TModel, TProperty>(expr, html.ViewData);

        //    if (metadata.AdditionalValues.ContainsKey("EditMask")) {
        //        return MvcHtmlString.Create((string)metadata.AdditionalValues["EditMask"]);
        //    }

        //    return MvcHtmlString.Create(String.Empty);
        //}
    }
}
