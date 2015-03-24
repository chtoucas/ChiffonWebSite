namespace Chiffon.Infrastructure
{
    using System;

    public static class PatternSizeExtensions
    {
        private static readonly PatternImageSize Original_ = new PatternImageSize(700, 520);
        private static readonly PatternImageSize Preview_ = new PatternImageSize(200, 160);

        public static PatternImageSize GetImageSize(this PatternSize @this)
        {
            switch (@this) {
                case PatternSize.Preview:
                    return Preview_;
                case PatternSize.Original:
                    return Original_;
                default:
                    throw new NotSupportedException("The requested size is not supported.");
            }
        }
    }
}
