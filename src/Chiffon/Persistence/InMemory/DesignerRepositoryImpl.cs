namespace Chiffon.Persistence.InMemory
{
    using System.Collections.Generic;
    using System.Linq;
    using Chiffon.Domain;
    using Narvalo.Collections;
    using Narvalo.Fx;

    public class DesignerRepositoryImpl : IDesignerRepository
    {
        static IEnumerable<Designer> Designers_
        {
            get
            {
                yield return new Designer {
                    DesignerId = new DesignerId(1),
                    DisplayName = "Chicamancha",
                    PatternDirectory = "chicamancha",
                    UrlKey = "chicamancha",
                };

                yield return new Designer {
                    DesignerId = new DesignerId(2),
                    DisplayName = "Viviane Devaux",
                    PatternDirectory = "viviane-devaux",
                    UrlKey = "viviane-devaux",
                };

                yield return new Designer {
                    DesignerId = new DesignerId(3),
                    DisplayName = "Christine Légeret",
                    PatternDirectory = "christine-legeret",
                    UrlKey = "christine-legeret",
                };

                yield return new Designer {
                    DesignerId = new DesignerId(4),
                    DisplayName = "Laure Roussel",
                    PatternDirectory = "laure-roussel",
                    UrlKey = "laure-roussel",
                };
            }
        }

        #region IDesignerRepository

        public IEnumerable<Designer> GetAll()
        {
            return Designers_;
        }

        public Maybe<Designer> GetDesigner(DesignerId designerId)
        {
            return (from _ in Designers_ where _.DesignerId == designerId select _).SingleOrNone();
        }

        #endregion
    }
}
