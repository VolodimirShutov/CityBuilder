using System;
using System.Collections.Generic;
using UnityEngine;

namespace City.Views
{
    public class ComponentProvisionService: IComponentProvisionService
    {
        private Dictionary<Type, IComponentProvider> _bindings = new Dictionary<Type, IComponentProvider>();

        public void Bind(IComponentProvider provider)
        {
            Debug.Log($"Provider of type {provider.GetType().Name} binded");
            
            var keyType = provider.GetType();
            _bindings[keyType] = provider;
        }
        
        public void Unbind(IComponentProvider provider)
        {
            var keyType = provider.GetType();
            _bindings.Remove(keyType);
        }

        public void BindRange(IComponentProvider[] providers)
        {
            for(var i=0;i!=providers.Length;i++)
                Bind(providers[i]);
        }
        
        public void UnbindRange(IComponentProvider[] providers)
        {
            for(var i=0;i!=providers.Length;i++)
                Unbind(providers[i]);
        }

        public TComponent Provide<TComponent, TProvider>()
            where TProvider: class, IComponentProvider<TComponent,TProvider>
            where TComponent: Component
        {
            var provider = GetProvider<TProvider>();
            return provider.Provide();
        }
        
        public TComponent Provide<TComponent>(Type providerType)
            where TComponent: Component
        {
            return GetProvider(providerType)?.Provide() as TComponent;
        }
        
        public Transform ProvideLocation<TProvider>()
            where TProvider: class, ILocationProvider, IComponentProvider<Transform,TProvider>
        {
            return Provide<Transform, TProvider>();
        }
        
        private TProvider GetProvider<TProvider>()
            where TProvider: class, IComponentProvider
        {
            return GetProvider(typeof(TProvider)) as TProvider;
        }
        
        private IComponentProvider GetProvider(Type providerType)
        {
            return _bindings.TryGetValue(providerType, out var provider)?  provider : null;
        }
    }
}