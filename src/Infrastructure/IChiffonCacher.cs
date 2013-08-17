namespace Chiffon.Infrastructure
{
    using System;

    public interface IChiffonCacher
    {
        T GetOrInsert<T>(string cacheKey, Func<T> query) where T : class;
    }
}