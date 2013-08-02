namespace Chiffon.Entities
{
    using System;
    using Narvalo;
    using Narvalo.Fx;

    [Serializable]
    public struct MemberId : IEquatable<MemberId>
    {
        readonly int _id;

        public MemberId(int id)
        {
            _id = id;
        }

        public int Id
        {
            get { return _id; }
        }

        #region IEquatable<MemberId>

        public bool Equals(MemberId other)
        {
            return _id == other._id;
        }

        #endregion

        public static bool operator ==(MemberId left, MemberId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MemberId left, MemberId right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MemberId)) {
                return false;
            }

            return Equals((MemberId)obj);
        }

        public override int GetHashCode()
        {
            return _id.GetHashCode();
        }
    }
}
