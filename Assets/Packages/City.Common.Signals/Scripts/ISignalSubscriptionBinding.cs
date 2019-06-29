namespace City.Common.Signals
{
    public interface ISignalSubscriptionBinding
    {
        ISignalSubscription[] Subscriptions { get; }
    }
}