using System;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public class ComponentProvider<TComponent, TSource>: IComponentProvider<TComponent, TSource>
        where TComponent: Component
        where TSource: class
    {
        public Type ComponentType { get; }
        public Func<TSource, TComponent> ProvidingMethod { get; private set; }
        
        public TSource _source;

        public ComponentProvider()
        {
            ComponentType = typeof(TComponent);
        }

        public void InitProvisionMethod(Func<TSource, TComponent> providingMethod)
        {
            ProvidingMethod = providingMethod;
        }
        
        public void InitSource(TSource source)
        {
            _source = source;
        }

        public TComponent Provide()
        {
            return ProvidingMethod.Invoke(_source);
        }

        Component IComponentProvider.Provide()
        {
            return Provide();
        }
    }
}