﻿namespace Chiffon.Infrastructure
{
    using System;

    public static class PatternSizeExtensions
    {
        private static readonly PatternImageSize s_Original = new PatternImageSize(700, 520);
        private static readonly PatternImageSize s_Preview = new PatternImageSize(200, 160);

        public static PatternImageSize GetImageSize(this PatternSize @this)
        {
            switch (@this) {
                case PatternSize.Preview:
                    return s_Preview;
                case PatternSize.Original:
                    return s_Original;
                default:
                    throw new NotSupportedException("The requested size is not supported.");
            }
        }
    }
}
