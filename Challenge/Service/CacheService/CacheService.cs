using Challenge.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;

namespace Challenge.Service.CacheService
{
    public sealed class CacheService : ICacheService
    {
        #region Private Fields

        private static readonly ConcurrentDictionary<string, CacheEntry> _cache;
        private static readonly ReaderWriterLockSlim _cacheLock;
        private static readonly int _maxAge; // in minutes
        private static readonly int _maxElements;
        private static readonly string _strategy; // "time" or "size"

        #endregion

        #region Constructor
        static CacheService()
        {
            _maxAge = int.Parse(ConfigurationManager.AppSettings["CacheMaxAge"] ?? "60");
            _maxElements = int.Parse(ConfigurationManager.AppSettings["CacheMaxElements"] ?? "100");
            _strategy = ConfigurationManager.AppSettings["CacheStrategy"] ?? "time"; ;
            _cache = new ConcurrentDictionary<string, CacheEntry>();
        }

        #endregion

        #region Public Methods
        public bool TryGetValue(string from, string to, DateTime start, DateTime end, out object value)
        {
            value = null;
            string key = GenerateCacheKey(from, to, start, end);

            try
            {
                if (_cache.TryGetValue(key, out var entry))
                {
                    // Check if cache entry is still valid (not expired by time-based strategy)
                    if (_strategy == "time" && DateTime.UtcNow.Subtract(entry.CreatedAt).TotalMinutes > _maxAge)
                    {
                        return false; // Expired
                    }

                    value = entry.Value;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public void Set(string from, string to, DateTime start, DateTime end, object value)
        {
            string key = GenerateCacheKey(from, to, start, end);

            try
            {
                _cache[key] = new CacheEntry { Value = value, CreatedAt = DateTime.UtcNow };
                CleanupCache();
            }
            catch
            {
            }
        }

        public void Clear()
        {
            try
            {
                _cache.Clear();
            }
            catch
            {

            }
        }

        #endregion

        #region Private Methods
        private void CleanupCache()
        {
            if (_strategy == "time")
            {
                var cutoffTime = DateTime.UtcNow.AddMinutes(-_maxAge);
                var expiredKeys = _cache
                    .Where(kvp => kvp.Value.CreatedAt < cutoffTime)
                    .Select(kvp => kvp.Key)
                    .ToList();

                foreach (var key in expiredKeys)
                {
                    _cache.TryRemove(key, out _);
                }
            }
            else if (_strategy == "size")
            {
                while (_cache.Count > _maxElements)
                {
                    var oldestKey = _cache
                        .OrderBy(kvp => kvp.Value.CreatedAt)
                        .First()
                        .Key;

                    _cache.TryRemove(oldestKey, out _);
                }
            }
        }

        private string GenerateCacheKey(string from, string to, DateTime start, DateTime end)
        {
            return $"{from}_{to}_{start:yyyy-MM-dd}_{end:yyyy-MM-dd}";
        }
        #endregion
    }
}
