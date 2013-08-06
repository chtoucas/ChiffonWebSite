namespace Chiffon.Entities
{
    using System;

    [Serializable]
    public struct CategoryKey : IEquatable<CategoryKey>
    {
        string _key;

        public CategoryKey(string key)
        {
            _key = key;
        }

        public string Key
        {
            get { return _key; }
        }

        #region IEquatable<CategoryKey>

        public bool Equals(CategoryKey other)
        {
            return _key == other._key;
        }

        #endregion

        public static bool operator ==(CategoryKey left, CategoryKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(CategoryKey left, CategoryKey right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is CategoryKey)) {
                return false;
            }

            return Equals((CategoryKey)obj);
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
