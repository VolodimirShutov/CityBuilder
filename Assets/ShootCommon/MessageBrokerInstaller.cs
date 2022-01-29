using System;
using UniRx;
using Packages.Utils.Scripts;
using Zenject;

namespace Common
{
    public class MessageBrokerInstaller: Installer<MessageBrokerInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IMessageBroker>()
                .FromInstance(MessageBroker.Default)
                .AsSingle();
            
            Container.Bind<IAsyncMessageBroker>()
                .FromInstance(AsyncMessageBroker.Default)
                .AsSingle();

            Container.Bind<ILateDisposable>()
                .FromMethod(context =>
                {
                    var messageBroker = context.Container.Resolve<IMessageBroker>() as IDisposable;
                    var asyncMessageBroker = context.Container.Resolve<IAsyncMessageBroker>() as IDisposable;
                    return new LateDisposableWrapper()
                        .Add(messageBroker)
                        .Add(asyncMessageBroker);
                }).AsSingle();
        }
    }
}