namespace Chiffon.Persistence
{
    using System.Linq;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class QueryService : IQueryService
    {
        readonly IDataContext _dataContext;

        public QueryService(IDataContext dataContext)
            : base()
        {
            Requires.NotNull(dataContext, "dataContext");

            _dataContext = dataContext;
        }

        public Maybe<QueryResults.MayGetPattern> MayGetPattern(
            string reference, string designerKey, bool publicOnly)
        {
            // select
            //  D.pattern_directory as directory
            //  , P.is_public       as is_public
            //  , P.reference       as reference
            // from Patterns as P
            //  inner join Designers as D on P.designer_id = D.id
            // where P.reference = @reference
            //  and (@public_only = 'false' or P.is_public)
            //  and D.urlkey = @designer_urlkey

            var q = from p in _dataContext.Patterns
                    join d in _dataContext.Designers on p.DesignerId equals d.DesignerId
                    where p.Reference == reference
                        && (!publicOnly || p.IsPublic)
                        && d.Key == designerKey
                    select new QueryResults.MayGetPattern {
                        Directory = d.Key,
                        IsPublic = p.IsPublic,
                        Reference = p.Reference
                    };

            return q.SingleOrNone();
        }
    }
}
