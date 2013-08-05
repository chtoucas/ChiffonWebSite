namespace Chiffon.Services
{
    using System.Collections.Generic;
    using Chiffon.Entities;

    public class InMemoryDataContext : IDataContext
    {
        #region IDataContext

        public IEnumerable<Designer> Designers
        {
            get
            {
                yield return new Designer(DesignerKey.Chicamancha) {
                    DisplayName = "Chicamancha",
                };

                yield return new Designer(DesignerKey.VivianeDevaux) {
                    DisplayName = "Viviane Devaux",
                };

                yield return new Designer(DesignerKey.ChristineLegeret) {
                    DisplayName = "Christine Légeret",
                };

                yield return new Designer(DesignerKey.LaureRoussel) {
                    DisplayName = "Laure Roussel",
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

        static Pattern CreateForChicamancha_(string reference, bool onDisplay = false)
        {
            return new Pattern(new PatternId(DesignerKey.Chicamancha, reference)) { OnDisplay = onDisplay };
        }

        static Pattern CreateForVivianeDevaux_(string reference, bool onDisplay = false)
        {
            return new Pattern(new PatternId(DesignerKey.VivianeDevaux, reference)) { OnDisplay = onDisplay };
        }

        static Pattern CreateForChristineLegeret_(string reference, bool onDisplay = false)
        {
            return new Pattern(new PatternId(DesignerKey.ChristineLegeret, reference)) { OnDisplay = onDisplay };
        }

        static Pattern CreateForLaureRoussel_(string reference, bool onDisplay = false)
        {
            return new Pattern(new PatternId(DesignerKey.LaureRoussel, reference)) { OnDisplay = onDisplay };
        }
    }
}
