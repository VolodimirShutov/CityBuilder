using System;
using UniRx;

namespace ShootCommon.Signals
{
    public interface ISignalPublisher
    {
        void Publish<TSignal>(TSignal signal = null)
            where TSignal: class, ISignal, new();
    }
}