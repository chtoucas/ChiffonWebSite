namespace Chiffon.Infrastructure
{
    using System;

    [Serializable]
    public struct PatternSize : IEquatable<PatternSize>
    {
        int _width;
        int _height;

        public PatternSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public int Width { get { return _width; } }
        public int Height { get { return _height; } }

        public override string ToString()
        {
            return base.ToString();
        }

        #region IEquatable<PatternSize>

        public bool Equals(PatternSize other)
        {
            return _width == other._width && _height == other._height;
        }

        #endregion

        public static bool operator ==(PatternSize left, PatternSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PatternSize left, PatternSize right)
        {
            return !left.Equals(right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PatternSize)) {
                return false;
            }

            return Equals((PatternSize)obj);
        }

        public override int GetHashCode()
        {
            // XXX
            return _width.GetHashCode() ^ _height.GetHashCode();
        }
    }
}
