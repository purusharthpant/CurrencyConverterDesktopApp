using Challenge.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Challenge.Service.CacheService
{
    public sealed class CacheService : ICacheService
    {
        private readonly Dictionary<string, CacheEntry> _cache;
        private readonly ReaderWriterLockSlim _cacheLock;
        private readonly int _maxAge; // in minutes
        private readonly int _maxElements;
        private readonly string _strategy; // "time" or "size"

        public CacheService(int maxAge, int maxElements, string strategy)
        {
            _maxAge = maxAge;
            _maxElements = maxElements;
            _strategy = strategy.ToLower();
            _cache = new Dictionary<string, CacheEntry>();
            _cacheLock = new ReaderWriterLockSlim();
        }

        private string GenerateCacheKey(string from, string to, DateTime start, DateTime end)
        {
            return $"{from}_{to}_{start:yyyy-MM-dd}_{end:yyyy-MM-dd}";
        }

        public bool TryGetValue(string from, string to, DateTime start, DateTime end, out object value)
        {
            value = null;
            string key = GenerateCacheKey(from, to, start, end);

            _cacheLock.EnterReadLock();
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
            finally
            {
                _cacheLock.ExitReadLock();
            }
        }

        public void Set(string from, string to, DateTime start, DateTime end, object value)
        {
            string key = GenerateCacheKey(from, to, start, end);

            _cacheLock.EnterWriteLock();
            try
            {
                _cache[key] = new CacheEntry { Value = value, CreatedAt = DateTime.UtcNow };
                CleanupCache();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }

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
                    _cache.Remove(key);
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

                    _cache.Remove(oldestKey);
                }
            }
        }

        public void Clear()
        {
            _cacheLock.EnterWriteLock();
            try
            {
                _cache.Clear();
            }
            finally
            {
                _cacheLock.ExitWriteLock();
            }
        }
    }
}
