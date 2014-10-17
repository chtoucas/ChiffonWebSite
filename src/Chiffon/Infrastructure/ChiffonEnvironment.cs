namespace Chiffon.Infrastructure
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Narvalo;

    public struct ChiffonEnvironment : IEquatable<ChiffonEnvironment>
    {
        Uri _baseUri;
        ChiffonLanguage _language;

        internal ChiffonEnvironment(ChiffonLanguage language, Uri baseUri)
        {
            Require.NotNull(baseUri, "baseUri");

            _language = language;
            _baseUri = baseUri;
        }

        /// <summary>
        /// Uri de base du site (sans le répertoire virtuel).
        /// </summary>
        public Uri BaseUri { get { return _baseUri; } }

        public ChiffonLanguage Language { get { return _language; } }

        #region IEquatable<ChiffonEnvironment>

        public bool Equals(ChiffonEnvironment other)
        {
            return _language.Equals(other._language);
        }

        #endregion

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
            if (!(obj is ChiffonEnvironment)) {
                return false;
            }

            return Equals((ChiffonEnvironment)obj);
        }

        public override int GetHashCode()
        {
            return _language.GetHashCode();
        }

        [SuppressMessage("Microsoft.Usage", "CA2234:PassSystemUriObjectsInsteadOfStrings"),
            SuppressMessage("Microsoft.Design", "CA1057:StringUriOverloadsCallSystemUriOverloads")]
        public Uri MakeAbsoluteUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }

        public Uri MakeAbsoluteUri(Uri relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }
    }
}