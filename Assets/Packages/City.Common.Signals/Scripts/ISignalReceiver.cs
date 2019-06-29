using System;
using UniRx;

namespace City.Common.Signals
{
    public interface ISignalReceiver
    {
        IObservable<TSignal> Receive<TSignal>()
            where TSignal: ISignal;

        IDisposable Subscribe<TSignal>(Func<TSignal, IObservable<Unit>> asyncMessageReceiver)
            where TSignal : IAsyncSignal;
    }
}