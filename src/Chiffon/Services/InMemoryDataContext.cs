namespace Chiffon.Services
{
    using System;
    using System.Collections.Generic;
    using System.Net.Mail;
    using Chiffon.Entities;

    public class InMemoryDataContext : IDataContext
    {
        const string LoremIpsum_ = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";

        #region IDataContext

        public IEnumerable<Designer> Designers
        {
            get
            {
                yield return new Designer(DesignerKey.Chicamancha) {
                    DisplayName = "Chicamancha",
                    EmailAddress = new MailAddress("chicamancha@hotmail.com"),
                    PreferredPatternReference = "1",
                    Presentation = LoremIpsum_,
                    WebSiteUrl = new Uri("http://www.chicamancha.com"),
                };

                yield return new Designer(DesignerKey.VivianeDevaux) {
                    DisplayName = "Viviane Devaux",
                    EmailAddress = new MailAddress("viviane.vdx@gmail.com"),
                    PreferredPatternReference = "1",
                    Presentation = LoremIpsum_,
                    WebSiteUrl = new Uri("http://www.vivianedevaux.com"),
                };

                yield return new Designer(DesignerKey.ChristineLegeret) {
                    DisplayName = "Christine Légeret",
                    EmailAddress = new MailAddress("christinelegeret@gmail.com"),
                    PreferredPatternReference = "1",
                    Presentation = LoremIpsum_,
                    WebSiteUrl = new Uri("http://www.christinelegeret.com"),
                };

                yield return new Designer(DesignerKey.LaureRoussel) {
                    DisplayName = "Laure Roussel",
                    EmailAddress = new MailAddress("laureroussel@gmail.com"),
                    PreferredPatternReference = "1",
                    Presentation = LoremIpsum_,
                    WebSiteUrl = new Uri("http://www.laureroussel.com"),
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

        static Pattern CreateForChicamancha_(string reference, bool showcased = false)
        {
            return new Pattern(new PatternId(DesignerKey.Chicamancha, reference)) { Showcased = showcased };
        }

        static Pattern CreateForVivianeDevaux_(string reference, bool showcased = false)
        {
            return new Pattern(new PatternId(DesignerKey.VivianeDevaux, reference)) { Showcased = showcased };
        }

        static Pattern CreateForChristineLegeret_(string reference, bool showcased = false)
        {
            return new Pattern(new PatternId(DesignerKey.ChristineLegeret, reference)) { Showcased = showcased };
        }

        static Pattern CreateForLaureRoussel_(string reference, bool showcased = false)
        {
            return new Pattern(new PatternId(DesignerKey.LaureRoussel, reference)) { Showcased = showcased };
        }
    }
}
