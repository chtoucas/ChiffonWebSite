namespace Chiffon.Infrastructure
{
    using System;
    using Narvalo;

    public class ChiffonEnvironment
    {
        readonly Uri _baseUri;
        readonly ChiffonCulture _culture;
        readonly ChiffonLanguage _language;

        public ChiffonEnvironment(ChiffonLanguage language, Uri baseUri)
        {
            Requires.NotNull(baseUri, "baseUri");

            _language = language;
            _baseUri = baseUri;
            _culture = ChiffonCulture.Create(Language);
        }

        public Uri BaseUri { get { return _baseUri; } }
        public ChiffonCulture Culture { get { return _culture; } }
        public ChiffonLanguage Language { get { return _language; } }
    }
}