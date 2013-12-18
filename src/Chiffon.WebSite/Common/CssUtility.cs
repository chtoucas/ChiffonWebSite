﻿namespace Chiffon.Common
{
    using System;
    using Chiffon.Entities;

    public static class CssUtility
    {
        public static string DesignerClass(DesignerKey key)
        {
            if (key == DesignerKey.ChristineLégeret) {
                return Constants.CssClassName.ChristineLégeret;
            }
            else if (key == DesignerKey.EstherMarthi) {
                return Constants.CssClassName.EstherMarthi;
            }
            else if (key == DesignerKey.LaureRoussel) {
                return Constants.CssClassName.LaureRoussel;
            }
            else if (key == DesignerKey.VivianeDevaux) {
                return Constants.CssClassName.VivianeDevaux;
            }
            else {
                throw new NotSupportedException();
            }
        }
    }
}