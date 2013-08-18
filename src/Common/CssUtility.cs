namespace Chiffon.Common
{
    using System;
    using Chiffon.Entities;

    public static class CssUtility
    {
        public static string DesignerClass(DesignerKey self)
        {
            if (self == DesignerKey.Chicamancha) {
                return CssClassName.Chicamancha;
            }
            else if (self == DesignerKey.ChristineLégeret) {
                return CssClassName.ChristineLégeret;
            }
            else if (self == DesignerKey.LaureRoussel) {
                return CssClassName.LaureRoussel;
            }
            else if (self == DesignerKey.VivianeDevaux) {
                return CssClassName.VivianeDevaux;
            }
            else {
                throw new NotSupportedException();
            }
        }
    }
}