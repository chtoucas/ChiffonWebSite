namespace Chiffon.Entities
{
    using System;
    using System.Diagnostics.Contracts;

    using Narvalo.Fx;

    public struct DesignerKey : IEquatable<DesignerKey>
    {
        public static readonly DesignerKey ChristineLégeret = new DesignerKey("petroleum-blue");
        public static readonly DesignerKey EstherMarthi = new DesignerKey("chicamancha");
        public static readonly DesignerKey LaureRoussel = new DesignerKey("laure-roussel");
        public static readonly DesignerKey VivianeDevaux = new DesignerKey("viviane-devaux");

        private readonly string _value;

        private DesignerKey(string value)
        {
            _value = value;
        }

        public string Value
        {
            get { return _value; }
        }

        // FIXME: This should return a nullable.
        public static Maybe<DesignerKey> MayParse(string value)
        {
            Contract.Ensures(Contract.Result<Maybe<DesignerKey>>() != null);

            switch (value)
            {
                case "chicamancha":
                    return Maybe.Of(EstherMarthi);
                case "viviane-devaux":
                    return Maybe.Of(VivianeDevaux);
                case "petroleum-blue":
                    return Maybe.Of(ChristineLégeret);
                case "laure-roussel":
                    return Maybe.Of(LaureRoussel);
                default:
                    return Maybe<DesignerKey>.None;
            }
        }

        public static DesignerKey Parse(string value)
        {
            switch (value)
            {
                case "chicamancha":
                    return EstherMarthi;
                case "viviane-devaux":
                    return VivianeDevaux;
                case "petroleum-blue":
                    return ChristineLégeret;
                case "laure-roussel":
                    return LaureRoussel;
                default:
                    throw new ArgumentException("The supplied value is not a valid designer key.", "value");
            }
        }

        public static bool operator ==(DesignerKey left, DesignerKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DesignerKey left, DesignerKey right)
        {
            return !left.Equals(right);
        }

        public bool Equals(DesignerKey other)
        {
            return _value == other._value;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DesignerKey))
            {
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
