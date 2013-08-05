namespace Chiffon.Services
{
    using System;
    using System.Collections.Generic;
    using Chiffon.Entities;
    using Chiffon.Infrastructure;
    using Narvalo.Fx;

    public interface IPatternService
    {
        IEnumerable<Pattern> GetShowcasedPatterns();
        Maybe<Pattern> MayGetPattern(PatternId patternId);
        Maybe<Tuple<PatternVisibility, PatternImage>> MayGetImage(PatternId patternId, PatternSize size);
    }
}
