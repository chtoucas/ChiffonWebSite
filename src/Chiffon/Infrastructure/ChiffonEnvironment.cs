namespace Chiffon.Infrastructure
{
    using System;
    using System.Globalization;
    using Narvalo;

    public struct ChiffonEnvironment : IEquatable<ChiffonEnvironment>
    {
        Uri _baseUri;
        CultureInfo _culture;
        CultureInfo _uiCulture;
        ChiffonLanguage _language;

        internal ChiffonEnvironment(ChiffonLanguage language, Uri baseUri)
        {
            Requires.NotNull(baseUri, "baseUri");

            _language = language;
            _baseUri = baseUri;
            _culture = language.GetCultureInfo();
            _uiCulture = language.GetUICultureInfo();
        }

        public Uri BaseUri { get { return _baseUri; } }
        public CultureInfo Culture { get { return _culture; } }
        public CultureInfo UICulture { get { return _uiCulture; } }
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
    }
}