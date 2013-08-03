namespace Chiffon.Persistence.InMemory
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Domain;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class PatternRepositoryImpl : IPatternRepository
    {
        static IEnumerable<Pattern> Patterns_
        {
            get
            {
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(1), "1"),
                };

                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(1), "2"),
                };

                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(1), "3"),
                };

                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(1), "4"),
                };
            }
        }

        #region IPatternRepository

        public IEnumerable<Pattern> GetAll()
        {
            return Patterns_;
        }

        public Maybe<Pattern> GetPattern(PatternId patternId)
        {
            return (from _ in Patterns_ where _.PatternId == patternId select _).SingleOrNone();
        }

        #endregion
    }
}
