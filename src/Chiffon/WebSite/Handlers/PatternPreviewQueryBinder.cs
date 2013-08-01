namespace Chiffon.WebSite.Handlers
{
    using System.Web;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternPreviewQueryBinder
    {
        public PatternPreviewQueryBinder() { }

        public Outcome<PatternPreviewQuery> Bind(HttpRequest request)
        {
            var query = request.QueryString;

            // > Paramètres obligatoires <

            var id = query.MayParseValue(PatternPreviewQuery.IdKey, _ => MayParse.ToInt32(_));
            if (id.IsNone) {
                return Outcome<PatternPreviewQuery>.Failure("XXX");
            }

            var width = query.MayParseValue(PatternPreviewQuery.WidthKey, _ => MayParse.ToInt32(_));
            if (width.IsNone) {
                return Outcome<PatternPreviewQuery>.Failure("XXX");
            }

            var height = query.MayParseValue(PatternPreviewQuery.HeightKey, _ => MayParse.ToInt32(_));
            if (height.IsNone) {
                return Outcome<PatternPreviewQuery>.Failure("XXX");
            }

            // > Création du modèle <

            var model = new PatternPreviewQuery {
                Height = height.Value,
                Id = id.Value,
                Width = width.Value,
            };

            return Outcome<PatternPreviewQuery>.Success(model);
        }
    }
}
