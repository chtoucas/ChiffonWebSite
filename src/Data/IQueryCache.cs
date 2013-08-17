namespace Chiffon.Data
{
    using System;

    public interface IQueryCache
    {
        T GetOrInsert<T>(string cacheKey, Func<T> query) where T : class;
    }
}