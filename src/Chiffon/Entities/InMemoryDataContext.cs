namespace Chiffon.Entities
{
    using System.Collections.Generic;

    public class InMemoryDataContext : IDataContext
    {
        #region IDataContext

        public IEnumerable<Designer> Designers
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

        public IEnumerable<Pattern> Patterns
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
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(2), "1"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(2), "2"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(2), "3"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(2), "4"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(3), "1"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(3), "2"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(3), "3"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(3), "4"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(4), "1"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(4), "2"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(4), "3"),
                };
                yield return new Pattern {
                    IsPublic = true,
                    PatternId = new PatternId(new DesignerId(4), "4"),
                };
            }
        }

        #endregion
    }
}
