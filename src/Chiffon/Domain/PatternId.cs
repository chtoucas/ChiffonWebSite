namespace Chiffon.Domain
{
    using System;
    using Narvalo;
    using Narvalo.Fx;

    [Serializable]
    public struct PatternId : IEquatable<PatternId>
    {
        readonly DesignerId _designerId;
        readonly string _reference;

        public PatternId(DesignerId designerId, string reference)
        {
            _designerId = designerId;
            _reference = reference;
        }

        public DesignerId DesignerId { get { return _designerId; } }
        public string Reference { get { return _reference; } }

        #region IEquatable<PatternId>

        public bool Equals(PatternId other)
        {
            return _designerId == other._designerId && _reference == other._reference;
        }

        #endregion

        public static bool operator ==(PatternId left, PatternId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PatternId left, PatternId right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PatternId)) {
                return false;
            }

            return Equals((PatternId)obj);
        }

        public override int GetHashCode()
        {
            return _designerId.GetHashCode() ^ _reference.GetHashCode();
        }
    }
}
