using Zenject;

namespace City.Common.Utils
{
    public interface ICache<TCached>
    {
        [Inject]
        void Init(TCached[] declaredCache);
        
        void Cache<TToCache>(TToCache cached)
            where TToCache : class, TCached;
        
        TToCache GetCached<TToCache>()
            where TToCache : class, TCached;
    }
}