namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public struct MemberKey : IEquatable<MemberKey>
    {
        string _key;

        public MemberKey(string key)
        {
            _key = key;
        }

        public string Key
        {
            get { return _key; }
        }

        #region IEquatable<MemberKey>

        public bool Equals(MemberKey other)
        {
            return _key == other._key;
        }

        #endregion

        public static bool operator ==(MemberKey left, MemberKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(MemberKey left, MemberKey right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is MemberKey)) {
                return false;
            }

            return Equals((MemberKey)obj);
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
