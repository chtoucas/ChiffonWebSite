﻿namespace Chiffon.Infrastructure
{
    using System.Globalization;
    using Narvalo;

    public class ChiffonCulture
    {
        readonly CultureInfo _culture;
        readonly CultureInfo _uiCulture;

        ChiffonCulture(CultureInfo culture, CultureInfo uiCulture)
        {
            Requires.NotNull(culture, "culture");
            Requires.NotNull(uiCulture, "uiCulture");

            _culture = culture;
            _uiCulture = uiCulture;
        }

        public CultureInfo Culture { get { return _culture; } }
        public CultureInfo UICulture { get { return _uiCulture; } }

        public string LanguageName
        {
            get { return UICulture.TwoLetterISOLanguageName; }
        }

        public static ChiffonCulture Create(ChiffonLanguage language)
        {
            switch (language) {
                case ChiffonLanguage.English:
                    return new ChiffonCulture(new CultureInfo("en-US"), new CultureInfo("en"));
                case ChiffonLanguage.Default:
                default:
                    return new ChiffonCulture(new CultureInfo("fr-FR"), new CultureInfo("fr-FR"));
            }
        }
    }
}