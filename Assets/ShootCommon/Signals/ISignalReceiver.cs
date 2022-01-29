using System;
using UniRx;

namespace ShootCommon.Signals
{
    public interface ISignalReceiver
    {
        IObservable<TSignal> Receive<TSignal>()
            where TSignal: ISignal;
    }
}