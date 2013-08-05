namespace Chiffon.Entities
{
    using System;
    using Narvalo;
    using Narvalo.Fx;

    [Serializable]
    public struct DesignerKey : IEquatable<DesignerKey>
    {
        public static readonly DesignerKey Chicamancha = new DesignerKey("chicamancha");
        public static readonly DesignerKey VivianeDevaux = new DesignerKey("viviane-devaux");
        public static readonly DesignerKey ChristineLegeret = new DesignerKey("christine-legeret");
        public static readonly DesignerKey LaureRoussel = new DesignerKey("laure-roussel");

        readonly string _key;

        DesignerKey(string key)
        {
            _key = key;
        }

        public string Key
        {
            get { return _key; }
        }

        public static Maybe<DesignerKey> Parse(string key)
        {
            switch (key) {
                case "chicamancha":
                    return Maybe.Create(Chicamancha);
                case "viviane-devaux":
                    return Maybe.Create(VivianeDevaux);
                case "christine-legeret":
                    return Maybe.Create(ChristineLegeret);
                case "laure-roussel":
                    return Maybe.Create(LaureRoussel);
                default:
                    return Maybe<DesignerKey>.None;
            }
        }

        #region IEquatable<MemberId>

        public bool Equals(DesignerKey other)
        {
            return _key == other._key;
        }

        #endregion

        public static bool operator ==(DesignerKey left, DesignerKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DesignerKey left, DesignerKey right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DesignerKey)) {
                return false;
            }

            return Equals((DesignerKey)obj);
        }

        public override int GetHashCode()
        {
            return _key.GetHashCode();
        }

        public override string ToString()
        {
            return _key;
        }
    }
}
