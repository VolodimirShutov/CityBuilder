namespace City.Common.Signals
{
    public interface ISignalSubscriptionFactory
    {
        ISignalSubscription CreateSubscription<TSignal, TCommand>()
            where TSignal : ISignal
            where TCommand : class;

        ISignalSubscription CreateAsyncSubscription<TSignal, TCommand>()
            where TSignal : IAsyncSignal
            where TCommand : class;
    }
}