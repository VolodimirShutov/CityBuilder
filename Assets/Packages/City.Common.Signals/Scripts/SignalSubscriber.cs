using System.Collections.Generic;

namespace City.Common.Signals
{
    public class SignalSubscriber<TSubscriber> 
        : ISignalSubscriber<TSubscriber>
    {
        private readonly List<ISignalSubscription> _subscriptions = new List<ISignalSubscription>();
        
        public void Init(ISignalSubscriptionBinding[] subscriptionBindings)
        {
            for (var i = 0; i != subscriptionBindings.Length; i++)
                _subscriptions.AddRange(subscriptionBindings[i].Subscriptions);    
        }
        
        public void Initialize()
        {
            for (var i = 0; i != _subscriptions.Count; i++)
                _subscriptions[i].Initialize();
        }

        public void Dispose()
        {
            for (var i = 0; i != _subscriptions.Count; i++)
                _subscriptions[i].Dispose();
        }
    }
}