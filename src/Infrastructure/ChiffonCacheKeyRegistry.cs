namespace Chiffon.Infrastructure
{
    using System;

    // TODO: À refaire correctement.
    public static class ChiffonCacheKeyRegistry
    {
        public static string GetCacheFormat(ChiffonCacheKey key)
        {
            switch (key) {
                case ChiffonCacheKey.GetDesignerViewModelQuery:
                    return "Chiffon:Designer:{0}:{1}";
                case ChiffonCacheKey.GetHomeViewModelQuery:
                    return "Chiffon:Home";
                case ChiffonCacheKey.ListPatternsQuery:
                    return "Chiffon:Pattern:{0}";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}