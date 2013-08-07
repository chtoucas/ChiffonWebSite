namespace Chiffon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternService : IPatternService
    {
        readonly IDataContext _dataContext;

        public PatternService(IDataContext dataContext)
            : base()
        {
            Requires.NotNull(dataContext, "dataContext");

            _dataContext = dataContext;
        }

        public IEnumerable<Pattern> GetShowcasedPatterns()
        {
            return from p in _dataContext.Patterns
                   where p.Showcased
                   select p;
        }

        public Maybe<Tuple<PatternVisibility, PatternImage>> MayGetImage(
            DesignerKey designerKey, string reference, PatternSize size)
        {
            var q = from p in _dataContext.Patterns
                    where p.DesignerKey == designerKey && p.Reference == reference
                    select p;

            return q.SingleOrNone().Map(_ => Tuple.Create(_.GetVisibility(size), _.GetImage(size)));
        }
    }
}
