namespace Chiffon.WebSite.Handlers
{
    using System.Web;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternQueryBinder
    {
        public PatternQueryBinder() { }

        public Outcome<PatternQuery> Bind(HttpRequest request)
        {
            var query = request.QueryString;

            // > Paramètres obligatoires <

            var id = query.MayParseValue(PatternQuery.IdKey, _ => MayParse.ToInt32(_));
            if (id.IsNone) {
                return Outcome<PatternQuery>.Failure(Error.Create("Id"));
            }

            // > Création du modèle <

            var model = new PatternQuery {
                Id = id.Value,
            };

            return Outcome<PatternQuery>.Success(model);
        }
    }
}
