using System;
using UniRx;

namespace City.Common.Signals
{
    public interface ISignalPublisher
    {
        void Publish<TSignal>(TSignal signal = null)
            where TSignal: class, ISignal, new();
        
        IObservable<Unit> PublishAsync<TSignal>(TSignal signal = null)
            where TSignal: class, IAsyncSignal, new();
    }
}