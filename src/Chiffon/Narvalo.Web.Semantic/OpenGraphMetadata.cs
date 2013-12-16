namespace Narvalo.Web.Semantic
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Narvalo;

    public class OpenGraphMetadata
    {
        public const string Namespace = "og: http://ogp.me/ns#";

        readonly OpenGraphLocale _locale;
        readonly Ontology _ontology;

        IList<OpenGraphLocale> _alternativeLocales = new List<OpenGraphLocale>();
        string _type = OpenGraphType.WebSite;

        OpenGraphImage _image;

        public OpenGraphMetadata(Ontology ontology)
        {
            Requires.NotNull(ontology, "ontology");

            _ontology = ontology;
            _locale = new OpenGraphLocale(ontology.Culture);
        }

        // > Paramètres obligatoires <

        public OpenGraphImage Image
        {
            get { return _image; }
            set { Requires.NotNull(value, "value"); _image = value; }
        }

        public string Title { get { return _ontology.Title; } }

        public string Type
        {
            get { return _type; }
            set { Requires.NotNullOrEmpty(value, "value"); _type = value; }
        }

        public Uri Url { get { return _ontology.Relationships.CanonicalUrl; } }

        // > Paramètres facultatifs <

        public string Description { get { return _ontology.Description; } }
        public string Determiner { get; set; }
        public OpenGraphLocale Locale { get { return _locale; } }

        public IReadOnlyCollection<OpenGraphLocale> AlternativeLocales
        {
            get
            {
                return new ReadOnlyCollection<OpenGraphLocale>(_alternativeLocales);
            }
        }

        public string SiteName { get; set; }

        public void AddAlternativeLocale(OpenGraphLocale locale)
        {
            _alternativeLocales.Add(locale);
        }
    }
}