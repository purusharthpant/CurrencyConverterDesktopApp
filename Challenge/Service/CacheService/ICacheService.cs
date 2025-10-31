using System;

namespace Challenge.Service.CacheService
{
    public interface ICacheService
    {
        /// <summary>
        /// Clears all cache entries.
        /// </summary>
        void Clear();

        /// <summary>
        /// Stores a value in the cache.
        /// Applies eviction strategy based on configuration.
        /// </summary>
        /// <param name="from">Source currency code</param>
        /// <param name="to">Target currency code</param>
        /// <param name="start">Start date for historical data</param>
        /// <param name="end">End date for historical data</param>
        /// <param name="value">The value to cache</param>
        void Set(string from, string to, DateTime start, DateTime end, object value);

        /// <summary>
        /// Attempts to retrieve a cached value for the given query parameters.
        /// </summary>
        /// <param name="from">Source currency code</param>
        /// <param name="to">Target currency code</param>
        /// <param name="start">Start date for historical data</param>
        /// <param name="end">End date for historical data</param>
        /// <param name="value">The cached value if found and valid</param>
        /// <returns>True if cache hit and entry is valid, false otherwise</returns>
        bool TryGetValue(string from, string to, DateTime start, DateTime end, out object value);
    }
}