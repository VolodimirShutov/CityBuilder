using System;
using Zenject;

namespace City.Common.Signals
{
    public interface ISignalSubscription: IInitializable, IDisposable
    {
    }
    
    public interface ISignalSubscription<TSignal>: ISignalSubscription
        where TSignal: ISignal
    {
    }
}