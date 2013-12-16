namespace Narvalo.Web.Semantic
{
    using System;
    using System.Globalization;
    using Narvalo;

    public class Ontology
    {
        readonly CultureInfo _culture;
        readonly Relationships _relationships = new Relationships();
        readonly SchemaOrgVocabulary _schemaOrg = new SchemaOrgVocabulary();
        readonly OpenGraphMetadata _openGraph;

        string _keywords = String.Empty;
        string _robotsDirective = "index, follow";

        public Ontology(CultureInfo culture)
        {
            Requires.NotNull(culture, "culture");

            _culture = culture;
            _openGraph = new OpenGraphMetadata(this);
        }

        public CultureInfo Culture { get { return _culture; } }

        public string LanguageName
        {
            get { return Culture.TwoLetterISOLanguageName; }
        }

        public OpenGraphMetadata OpenGraph
        {
            get { return _openGraph; }
        }

        public Relationships Relationships
        {
            get { return _relationships; }
        }

        public SchemaOrgVocabulary SchemaOrg
        {
            get { return _schemaOrg; }
        }

        public string RobotsDirective { get { return _robotsDirective; } set { _robotsDirective = value; } }

        public string Description { get; set; }

        public string Keywords { get; set; }

        public string Title { get; set; }
    }
}