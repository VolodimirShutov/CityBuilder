using System;
using UniRx;

namespace City.Common.Signals
{
    public class AsyncSignalSubscription<TSignal>: ISignalSubscription<TSignal>
        where TSignal: IAsyncSignal
    {
        private readonly Func<TSignal, IObservable<Unit>> _doOnSignalObservableFactoryMethod;
        private readonly IAsyncMessageBroker _asyncMessageBroker;
        private IDisposable _internalSubscription;
        
        public AsyncSignalSubscription(IAsyncMessageBroker messageBroker, Func<TSignal, IObservable<Unit>> doOnSignal)
        {
            _asyncMessageBroker = messageBroker;
            _doOnSignalObservableFactoryMethod = doOnSignal;
        }

        public void Initialize()
        {
            _internalSubscription = _asyncMessageBroker.Subscribe<TSignal>(_doOnSignalObservableFactoryMethod);
        }
        
        public void Dispose()
        {
            _internalSubscription?.Dispose();
            _internalSubscription = null;
        }
    }
}