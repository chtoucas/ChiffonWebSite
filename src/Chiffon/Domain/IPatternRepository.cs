namespace Chiffon.Domain
{
    using System.Collections.Generic;
    using Narvalo.Fx;

    public interface IPatternRepository
    {
        IEnumerable<Pattern> GetAll();
        Maybe<Pattern> GetPattern(PatternId patternId);
    }
}
