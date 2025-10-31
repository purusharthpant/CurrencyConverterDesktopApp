using System;

namespace Challenge.Service
{
    public interface ICacheService
    {
        void Clear();
        void Set(string from, string to, DateTime start, DateTime end, object value);
        bool TryGetValue(string from, string to, DateTime start, DateTime end, out object value);
    }
}