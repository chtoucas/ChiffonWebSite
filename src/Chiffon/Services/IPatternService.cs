namespace Chiffon.Services
{
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Narvalo.Fx;

    public interface IPatternService
    {
        IEnumerable<Pattern> GetShowcasedPatterns();
        Maybe<Pattern> MayGetPattern(PatternId patternId);
    }
}
