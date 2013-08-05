namespace Chiffon.Services
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Entities;
    using Narvalo;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PaternService : IPatternService
    {
        readonly IDataContext _dataContext;

        public PaternService(IDataContext dataContext)
            : base()
        {
            Requires.NotNull(dataContext, "dataContext");

            _dataContext = dataContext;
        }

        public IEnumerable<Pattern> GetPatternsOnDisplay()
        {
            return from p in _dataContext.Patterns
                   where p.OnDisplay
                   select p;
        }

        public Maybe<Pattern> MayGetPattern(PatternId patternId)
        {
            var q = from p in _dataContext.Patterns
                    where p.PatternId == patternId
                    select p;

            return q.SingleOrNone();
        }
    }
}
