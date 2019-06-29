using System;
using UniRx;
using Zenject;

namespace City.Common.Signals
{
    public class SignalSubscription<TSignal>: ISignalSubscription<TSignal>
        where TSignal: ISignal
    {
        private readonly IMessageBroker _messageBroker;
        private readonly Action<TSignal> _doOnSignal;
        private IDisposable _internalSubscription;
        
        public SignalSubscription(IMessageBroker messageBroker, Action<TSignal> doOnSignal)
        {
            _messageBroker = messageBroker;
            _doOnSignal = doOnSignal;
        }

        public void Initialize()
        {
            _internalSubscription = _messageBroker.Receive<TSignal>()
                .Do(_doOnSignal)
                .Subscribe();
        }
        
        public void Dispose()
        {
            _internalSubscription?.Dispose();
            _internalSubscription = null;
        }
    }
}