using System;
using City.Common.Signals;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public static class MediationContainerExtensions
    {
        public static void BindViewToAnchor<TView, TAnchor>(this DiContainer container)
            where TView : Component, IView
            where TAnchor : class, ILocationProvider
        {
            container.BindPrefabToAnchor<TView, TAnchor>();
        }

        public static void BindViewToRoot<TView, TAnchor>(this DiContainer container)
            where TView : Component, IView
            where TAnchor : class, ILocationProvider
        {
            container.BindPrefabToRoot<TView, TAnchor>();
        }
        
        public static void DisableViewOnInitialization<TView>(this DiContainer container)
            where TView: Component, IView
        {
            container.BindVisitorToView<DisableViewOnInitializationMediatorVisitor<TView>, TView>();
        }
        
        public static void BindViewAsSourceProvider<TView>(this DiContainer container)
            where TView: Component, IView
        {
            container.BindVisitorToView<SourceProviderMediatorVisitor<TView>, TView>();
        }
        
        public static void BindViewAsSignalSubscriber<TView>(this DiContainer container)
            where TView: Component, IView
        {
            container.BindVisitorToView<SignalSubscriberMediatorVisitor<TView>, TView>();
        }
        
        public static void BindSignalToView<TSignal, TView>(this DiContainer container)
            where TView: Component
            where TSignal: ISignal
        {
            container.BindSignalToExternalSubscriber<TSignal, TView>();
        }

        public static void BindVisitorToView<TVisitor, TView>(this DiContainer container)
            where TVisitor: IMediatorVisitor<TView>
            where TView : Component, IView
        {
            container.Bind<IMediatorVisitor<TView>>().To<TVisitor>()
                .FromMethod(context => InstantiateVisitor<TView, TVisitor>(context))
                .WhenInjectedInto<IMediator<TView>>();
        }


        public static void BindViewToMediator<TView, TMediator>(this DiContainer container, int disposableExecutionOrder = 0)
            where TView : Component, IView
            where TMediator : IMediator
        {
            container.Bind<IMediator>().To<TMediator>().FromMethod(context =>
                    InstantiateMediator<TView, TMediator>(context, disposableExecutionOrder))
                .WhenInjectedInto<TView>();

            if (disposableExecutionOrder != 0)
                container.BindDisposableExecutionOrder<TMediator>(disposableExecutionOrder);
        }

        private static TMediator InstantiateMediator<TView, TMediator>(InjectContext context, int disposableExecutionOrder = 0)
            where TView : Component, IView
            where TMediator : IMediator
        {
            
            var view = context.ObjectInstance as TView;

            if (view == null)
                throw new InvalidOperationException(
                    $"Can't instantiate {typeof(TMediator).Name} for {context.ObjectInstance.GetType().Name}");
            
            var args = new[] {(object)view};
            var instance = context.InstantiateWithArgsInternal<TView, TMediator>(view.gameObject, args);
            
            var disposableManager = context.Container.Resolve<DisposableManager>();
            disposableManager.Add(instance, disposableExecutionOrder);
            
            (instance as IInitializable)?.Initialize(); 
            
            return instance;
        }
        
        private static TVisitor InstantiateVisitor<TView, TVisitor>(InjectContext context)
            where TView : Component, IView
            where TVisitor: IMediatorVisitor
        {
            
            var mediator = context.ObjectInstance as IMediator<TView>;

            if (mediator == null || mediator.View == null)
                throw new InvalidOperationException(
                    $"Can't instantiate {typeof(TVisitor).Name} for {context.ObjectInstance.GetType().Name}");

            var args = new[] {(object)mediator};
            
            var instance = context.InstantiateWithArgsInternal<TView, TVisitor>(mediator.View.gameObject, args);

            return instance;
        }

        private static T InstantiateWithArgsInternal<TView, T>(this InjectContext context, GameObject gameObject, object[] args)
            where TView : Component, IView
        {
            var instance =  (typeof(T).IsSubclassOf(typeof(Component)))
                ? context.Container.InstantiateComponent(typeof(T), gameObject, args)
                    .GetComponent<T>()
                : context.Container.Instantiate<T>(args);

            return instance;
        }
    }
}