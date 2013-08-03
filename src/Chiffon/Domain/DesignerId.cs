namespace Chiffon.Domain
{
    using System;
    using Narvalo;
    using Narvalo.Fx;

    [Serializable]
    public struct DesignerId : IEquatable<DesignerId>
    {
        readonly int _id;

        public DesignerId(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }

        #region IEquatable<MemberId>

        public bool Equals(DesignerId other)
        {
            return _id == other._id;
        }

        #endregion

        public static bool operator ==(DesignerId left, DesignerId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DesignerId left, DesignerId right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DesignerId)) {
                return false;
            }

            return Equals((DesignerId)obj);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
