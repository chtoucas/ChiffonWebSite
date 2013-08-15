namespace Chiffon.Infrastructure
{
    using System;
    using Narvalo;

    public abstract class ChiffonEnvironmentBase
    {
        readonly Uri _baseUri;
        readonly ChiffonCulture _culture;

        protected ChiffonEnvironmentBase(ChiffonCulture culture, Uri baseUri)
        {
            Requires.NotNull(culture, "culture");
            Requires.NotNull(baseUri, "baseUri");

            _baseUri = baseUri;
            _culture = culture;
        }

        public Uri BaseUri { get { return _baseUri; } }

        public ChiffonCulture Culture { get { return _culture; } }

        public abstract void Initialize();
    }
}