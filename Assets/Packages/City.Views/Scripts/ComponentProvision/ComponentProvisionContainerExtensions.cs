using System;
using UniRx;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public static class ComponentProvisionContainerExtensions
    {
     
        public static void BindLocationProvider<TProvider, TSource>(this DiContainer container,
            Func<TSource, Transform> providingMethod)
            where TProvider: class, IComponentProvider<Transform,TSource>, ILocationProvider, new()
            where TSource: class
        {
            container.BindComponentProvider<TProvider, Transform, TSource>(providingMethod);
        }
        
        public static void BindCameraProvider<TProvider, TSource>(this DiContainer container,
            Func<TSource, Camera> providingMethod)
            where TProvider: class, IComponentProvider<Camera,TSource>, ICameraProvider, new()
            where TSource: class
        {
            container.BindComponentProvider<TProvider, Camera, TSource>(providingMethod);
        }
        
        public static void BindComponentProvider<TProvider, TComponent, TSource>(this DiContainer container, 
            Func<TSource, TComponent> providingMethod)
            where TComponent: Component
            where TProvider: IComponentProvider<TComponent,TSource>, new()
            where TSource: class
        {
            container.Bind<IProviderWithSource<TSource>>().FromMethod(() =>
                {
                    var provider = new TProvider(); 
                    provider.InitProvisionMethod(providingMethod);
                    return provider;
                })
                .WhenInjectedInto<IComponentSourceProvider<TSource>>();
        }

        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindCameraFromProvider<TProvider>(
            this DiContainer container, object id = null)
            where TProvider : ICameraProvider, new()
        {
            return container.BindComponentFromProvider<Camera, TProvider>(id);
        }
        
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindLocationFromProvider<TProvider>(
            this DiContainer container, object id = null)
            where TProvider : ILocationProvider, new()
        {
            return container.BindComponentFromProvider<Transform, TProvider>(id);
        }
        
        public static ScopeConcreteIdArgConditionCopyNonLazyBinder BindComponentFromProvider<TComponent, TProvider>(
            this DiContainer container, object id = null)
            where TComponent : Component
            where TProvider : IComponentProvider<TComponent>, new()
        {
            return container.Bind<TComponent>().WithId(id)
                .FromResolveGetter<IComponentProvisionService>(service =>
                    service.Provide<TComponent>(typeof(TProvider)));
        }
    }
}