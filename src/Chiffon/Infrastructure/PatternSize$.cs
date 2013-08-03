namespace Chiffon.Infrastructure
{
    using System;

    public static class PatternSizeExtensions
    {
        static readonly PatternImageSize Original_ = new PatternImageSize(700, 520);
        static readonly PatternImageSize Preview_ = new PatternImageSize(200, 160);

        public static PatternImageSize GetImageSize(this PatternSize size)
        {
            switch (size) {
                case PatternSize.Preview:
                    return Preview_;
                case PatternSize.Original:
                    return Original_;
                default:
                    throw new ArgumentException();
            }
        }
    }
}
