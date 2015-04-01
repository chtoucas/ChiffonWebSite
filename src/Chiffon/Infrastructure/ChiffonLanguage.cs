﻿namespace Chiffon.Infrastructure
{
    using System.Globalization;

    public enum ChiffonLanguage
    {
        Default = 0,
        English = 1,
        French = Default,
    }

    public static class ChiffonLanguageExtensions
    {
        public static bool IsDefault(this ChiffonLanguage @this)
        {
            return @this == ChiffonLanguage.Default;
        }

        internal static CultureInfo GetCultureInfo(this ChiffonLanguage @this)
        {
            switch (@this) {
                case ChiffonLanguage.English:
                    return new CultureInfo("en");
                case ChiffonLanguage.Default:
                default:
                    return new CultureInfo("fr");
            }
        }

        internal static CultureInfo GetUICultureInfo(this ChiffonLanguage @this)
        {
            switch (@this) {
                case ChiffonLanguage.English:
                    return new CultureInfo("en");
                case ChiffonLanguage.Default:
                default:
                    return new CultureInfo("fr");
            }
        }
    }
}