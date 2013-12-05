namespace Chiffon.Infrastructure
{
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
    }
}