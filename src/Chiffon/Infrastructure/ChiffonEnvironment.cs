namespace Chiffon.Infrastructure
{
    using System;

    using Narvalo;

    public struct ChiffonEnvironment : IEquatable<ChiffonEnvironment>
    {
        Uri _baseUri;
        ChiffonHosting _hosting;
        ChiffonLanguage _language;

        internal ChiffonEnvironment(ChiffonLanguage language, Uri baseUri, ChiffonHosting hosting)
        {
            Require.NotNull(baseUri, "baseUri");

            _language = language;
            _baseUri = baseUri;
            _hosting = hosting;
        }

        /// <summary>
        /// Uri de base du site (sans le répertoire virtuel).
        /// </summary>
        public Uri BaseUri { get { return _baseUri; } }

        public ChiffonHosting Hosting { get { return _hosting; } }

        public ChiffonLanguage Language { get { return _language; } }

        public bool Equals(ChiffonEnvironment other)
        {
            return _language.Equals(other._language);
        }

        public static bool operator ==(ChiffonEnvironment left, ChiffonEnvironment right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ChiffonEnvironment left, ChiffonEnvironment right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ChiffonEnvironment))
            {
                return false;
            }

            return Equals((ChiffonEnvironment)obj);
        }

        public override int GetHashCode()
        {
            // TODO: Dubious.
            return _language.GetHashCode()
                ^ _baseUri.GetHashCode()
                ^ _hosting.GetHashCode();
        }

        public Uri MakeAbsoluteUri(string relativePath)
        {
            return new Uri(_baseUri, relativePath);
        }

        public Uri MakeAbsoluteUri(Uri relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }
    }
}