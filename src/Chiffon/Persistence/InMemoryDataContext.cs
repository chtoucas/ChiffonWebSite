namespace Chiffon.Persistence
{
    using System.Collections.Generic;
    using Chiffon.Entities;

    public class InMemoryDataContext : IDataContext
    {
        static readonly DesignerId ChicamanchaId_ = new DesignerId(1);
        static readonly DesignerId VivianeDevauxId_ = new DesignerId(2);
        static readonly DesignerId ChristineLegeretId_ = new DesignerId(3);
        static readonly DesignerId LaureRousselId_ = new DesignerId(4);

        #region IDataContext

        public IEnumerable<Designer> Designers
        {
            get
            {
                yield return new Designer(ChicamanchaId_) {
                    DisplayName = "Chicamancha",
                    Key = "chicamancha",
                };

                yield return new Designer(VivianeDevauxId_) {
                    DisplayName = "Viviane Devaux",
                    Key = "viviane-devaux",
                };

                yield return new Designer(ChristineLegeretId_) {
                    DisplayName = "Christine Légeret",
                    Key = "christine-legeret",
                };

                yield return new Designer(LaureRousselId_) {
                    DisplayName = "Laure Roussel",
                    Key = "laure-roussel",
                };
            }
        }

        public IEnumerable<Pattern> Patterns
        {
            get
            {
                yield return CreateForChicamancha_("1", true);
                yield return CreateForChicamancha_("2", true);
                yield return CreateForChicamancha_("3", true);
                yield return CreateForChicamancha_("4", true);
                yield return CreateForChicamancha_("5");
                yield return CreateForChicamancha_("6");

                yield return CreateForVivianeDevaux_("1", true);
                yield return CreateForVivianeDevaux_("2", true);
                yield return CreateForVivianeDevaux_("3", true);
                yield return CreateForVivianeDevaux_("4", true);
                yield return CreateForVivianeDevaux_("5");
                yield return CreateForVivianeDevaux_("6");

                yield return CreateForChristineLegeret_("1", true);
                yield return CreateForChristineLegeret_("2", true);
                yield return CreateForChristineLegeret_("3", true);
                yield return CreateForChristineLegeret_("4", true);
                yield return CreateForChristineLegeret_("5");
                yield return CreateForChristineLegeret_("6");

                yield return CreateForLaureRoussel_("1", true);
                yield return CreateForLaureRoussel_("2", true);
                yield return CreateForLaureRoussel_("3", true);
                yield return CreateForLaureRoussel_("4", true);
                yield return CreateForLaureRoussel_("5");
                yield return CreateForLaureRoussel_("6");
            }
        }

        #endregion

        static Pattern CreateForChicamancha_(string reference, bool isPublic = false)
        {
            return new Pattern(new PatternId(ChicamanchaId_, reference)) { IsPrivate = !isPublic };
        }

        static Pattern CreateForVivianeDevaux_(string reference, bool isPublic = false)
        {
            return new Pattern(new PatternId(VivianeDevauxId_, reference)) { IsPrivate = !isPublic };
        }

        static Pattern CreateForChristineLegeret_(string reference, bool isPublic = false)
        {
            return new Pattern(new PatternId(ChristineLegeretId_, reference)) { IsPrivate = !isPublic };
        }

        static Pattern CreateForLaureRoussel_(string reference, bool isPublic = false)
        {
            return new Pattern(new PatternId(LaureRousselId_, reference)) { IsPrivate = !isPublic };
        }
    }
}
