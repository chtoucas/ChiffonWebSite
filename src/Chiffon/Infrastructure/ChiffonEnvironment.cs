namespace Chiffon.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using Narvalo;

    public struct ChiffonEnvironment
    {
        static readonly ChiffonEnvironment Default_
            = new ChiffonEnvironment(ChiffonLanguage.Default, new Uri("http://pourquelmotifsimone.com"));

        public static IEnumerable<ChiffonEnvironment> Environments
        {
            get
            {
                yield return Default_;
                yield return new ChiffonEnvironment(
                    ChiffonLanguage.English, new Uri("http://en.pourquelmotifsimone.com"));
            }
        }

        Uri _baseUri;
        ChiffonCulture _culture;
        ChiffonLanguage _language;

        public ChiffonEnvironment(ChiffonLanguage language, Uri baseUri)
        {
            Requires.NotNull(baseUri, "baseUri");

            _language = language;
            _baseUri = baseUri;
            _culture = ChiffonCulture.Create(_language);
        }

        public static ChiffonEnvironment Default
        {
            get { return Default_; }
        }

        public Uri BaseUri { get { return _baseUri; } }
        public ChiffonCulture Culture { get { return _culture; } }
        public ChiffonLanguage Language { get { return _language; } }
    }
}