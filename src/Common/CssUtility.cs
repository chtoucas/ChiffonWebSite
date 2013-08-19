namespace Chiffon.Common
{
    using System;
    using Chiffon.Entities;

    public static class CssUtility
    {
        public static string DesignerClass(DesignerKey key)
        {
            if (key == DesignerKey.ChristineLégeret) {
                return CssClassName.ChristineLégeret;
            }
            else if (key == DesignerKey.EstherMarthi) {
                return CssClassName.EstherMarthi;
            }
            else if (key == DesignerKey.LaureRoussel) {
                return CssClassName.LaureRoussel;
            }
            else if (key == DesignerKey.VivianeDevaux) {
                return CssClassName.VivianeDevaux;
            }
            else {
                throw new NotSupportedException();
            }
        }
    }
}