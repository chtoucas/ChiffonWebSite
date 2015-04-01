namespace Chiffon.Infrastructure
{
    using System;

    public struct PatternImageSize : IEquatable<PatternImageSize>
    {
        private int _width;
        private int _height;

        public PatternImageSize(int width, int height)
        {
            _width = width;
            _height = height;
        }

        public double AspectRatio { get { return _width / _height; } }

        public int Height { get { return _height; } }

        public int Width { get { return _width; } }

        public static bool operator ==(PatternImageSize left, PatternImageSize right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(PatternImageSize left, PatternImageSize right)
        {
            return !left.Equals(right);
        }

        public bool Equals(PatternImageSize other)
        {
            return _width == other._width && _height == other._height;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PatternImageSize))
            {
                return false;
            }

            return Equals((PatternImageSize)obj);
        }

        public override int GetHashCode()
        {
            return _width.GetHashCode() ^ _height.GetHashCode();
        }
    }
}