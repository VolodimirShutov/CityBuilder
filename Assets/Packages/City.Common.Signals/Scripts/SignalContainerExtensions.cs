using System;
using UnityEngine;
using Zenject;

namespace City.Common.Signals
{
    public static class SignalContainerExtensions
    {
        public static void BindSignalToExternalSubscriber<TSignal, TSubscriber>(this DiContainer container)
            where TSignal: ISignal
        {
            container.Bind<ISignalSubscriptionBinding>()
                .To<SignalSubscriptionBinding<TSignal>>()
                .FromNew()
                .WhenInjectedInto<ISignalSubscriber<TSubscriber>>();
        }
        
        public static void BindSignalToSubscriber<TSignal, TSubscriber>(this DiContainer container)
            where TSubscriber : ISignalSubscriber
            where TSignal: ISignal
        {
            container.Bind<ISignalSubscriptionBinding>()
                .To<SignalSubscriptionBinding<TSignal>>()
                .FromNew()
                .WhenInjectedInto<TSubscriber>();
        }
        
        public static void SubscribeToSignal<TSignal,TCommand>(
            this DiContainer container,
            Action<TCommand> setupCommand = null)
            where TSignal : ISignal
            where TCommand : class
        {
            container.Bind<ISignalSubscription>()
                .To<ISignalSubscription>()
                .FromResolveGetter<ISignalSubscriptionFactory>(factory =>
                    {
                        Debug.Log($"Command {typeof(TCommand).Name} subscribed to {typeof(TSignal).Name}");
                        return factory.CreateSubscription<TSignal, TCommand>();
                    })
                .WhenInjectedInto<SignalSubscriptionBinding<TSignal>>();
        }
        
        public static void SubscribeToAsyncSignal<TSignal, TCommand>(
            this DiContainer container)
            where TSignal : IAsyncSignal
            where TCommand : class
        {
            container.Bind<ISignalSubscription>()
                .To<ISignalSubscription>()
                .FromResolveGetter<ISignalSubscriptionFactory>(factory =>
                {
                    Debug.Log($"Command {typeof(TCommand).Name} subscribed to {typeof(TSignal).Name}");
                    return factory.CreateAsyncSubscription<TSignal,TCommand>();
                })
                .WhenInjectedInto<SignalSubscriptionBinding<TSignal>>();
        }
        
        public static void BindSignalSubscriber<TSubscriber>(this DiContainer container)
        {
            container.Bind<ISignalSubscriber<TSubscriber>>()
                .To<SignalSubscriber<TSubscriber>>()
                .FromNew()
                .AsTransient();
        }
    }
}