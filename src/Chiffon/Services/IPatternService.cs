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

        Maybe<Tuple<PatternVisibility, PatternImage>> MayGetImage(
            DesignerKey designerKey, string reference, PatternSize size);
    }
}
