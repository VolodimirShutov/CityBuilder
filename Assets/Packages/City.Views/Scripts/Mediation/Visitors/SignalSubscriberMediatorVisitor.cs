using City.Common.Signals;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public class SignalSubscriberMediatorVisitor<TView>: MediatorVisitor<TView>, ISignalSubscriber<TView>
        where TView: Component, IView
    {
        private ISignalSubscriber _subscriber;

        [Inject]
        public void Init(ISignalSubscriptionBinding[] subscriptions)
        {
            _subscriber = new SignalSubscriber<TView>();
            _subscriber.Init(subscriptions);
        }

        public override void Initialize()
        {
            _subscriber.Initialize();
        }

        public override void Dispose()
        {
            _subscriber.Dispose();
        }
    }
}