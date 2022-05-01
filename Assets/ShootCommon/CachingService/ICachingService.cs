using System;

namespace ShootCommon.CachingService
{
    public interface ICachingService
    {
        void Save<T>(string key, T value, Action callback) where T : class;
        void Read<T>(string key, Action<T> callback) where T : class;
    }
}