using System;
using UnityEngine;

namespace City.Views
{
    public interface IComponentProvider
    {
        Component Provide();  
    }

    public interface IComponentProvider<TComponent>: IComponentProvider
        where TComponent : Component
    {
        TComponent Provide();    
    }
    
    public interface IComponentProvider<TComponent, TSource>: 
        IProviderWithSource<TSource>, 
        IComponentProvider<TComponent>
        where TSource: class
        where TComponent: Component
    {
        void InitProvisionMethod(Func<TSource, TComponent> providingMethod);
        
    }
}