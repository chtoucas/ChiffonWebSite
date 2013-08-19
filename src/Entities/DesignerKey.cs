namespace Chiffon.Entities
{
    using System;
    using Narvalo.Fx;

    [Serializable]
    public struct DesignerKey : IEquatable<DesignerKey>
    {
        public static readonly DesignerKey ChristineLégeret = new DesignerKey("petroleum-blue");
        public static readonly DesignerKey EstherMarthi = new DesignerKey("chicamancha");
        public static readonly DesignerKey LaureRoussel = new DesignerKey("laure-roussel");
        public static readonly DesignerKey VivianeDevaux = new DesignerKey("viviane-devaux");

        string _value;

        DesignerKey(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

        public static Maybe<DesignerKey> MayParse(string value)
        {
            switch (value) {
                case "chicamancha":
                    return Maybe.Create(EstherMarthi);
                case "viviane-devaux":
                    return Maybe.Create(VivianeDevaux);
                case "petroleum-blue":
                    return Maybe.Create(ChristineLégeret);
                case "laure-roussel":
                    return Maybe.Create(LaureRoussel);
                default:
                    return Maybe<DesignerKey>.None;
            }
        }

        public static DesignerKey Parse(string value)
        {
            switch (value) {
                case "chicamancha":
                    return EstherMarthi;
                case "viviane-devaux":
                    return VivianeDevaux;
                case "petroleum-blue":
                    return ChristineLégeret;
                case "laure-roussel":
                    return LaureRoussel;
                default:
                    throw new ArgumentException();
            }
        }

        #region IEquatable<DesignerKey>

        public bool Equals(DesignerKey other)
        {
            return _value == other._value;
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
            return _value.GetHashCode();
        }

        public override string ToString()
        {
            return _value;
        }
    }
}
