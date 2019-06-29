
using UnityEngine;

namespace City.Common.Signals
{
    public class SignalSubscriptionBinding<TSignal>: ISignalSubscriptionBinding
        where TSignal: ISignal
    {
        public ISignalSubscription[] Subscriptions { get; }
        
        public SignalSubscriptionBinding(ISignalSubscription[] subscriptions)
        {
            Debug.Log($"SignalSubscriptionBinding created for {typeof(TSignal).Name} with {subscriptions.Length} subscriptions");
            
            Subscriptions = new ISignalSubscription[subscriptions.Length];

            for (var i = 0; i != subscriptions.Length; i++)
                Subscriptions[i] = subscriptions[i];
        }
    }
}