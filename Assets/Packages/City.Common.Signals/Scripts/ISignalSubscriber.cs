using System;
using Zenject;

namespace City.Common.Signals
{
    public interface ISignalSubscriber: IInitializable, IDisposable
    {
        [Inject]
        void Init(ISignalSubscriptionBinding[] subscriptions);
    }
    
    public interface ISignalSubscriber<TSubscriber>: ISignalSubscriber
    {
    }
}