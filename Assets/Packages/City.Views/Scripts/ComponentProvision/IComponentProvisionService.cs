using System;
using UnityEngine;

namespace City.Views
{
    public interface IComponentProvisionService
    {

        void Bind(IComponentProvider provider);
        void Unbind(IComponentProvider provider);
        void BindRange(IComponentProvider[] providers);
        void UnbindRange(IComponentProvider[] providers);

        TComponent Provide<TComponent, TProvider>()
            where TProvider : class, IComponentProvider<TComponent, TProvider>
            where TComponent : Component;
        
        TComponent Provide<TComponent>(Type providerType)
            where TComponent : Component;

        Transform ProvideLocation<TProvider>()
            where TProvider : class, ILocationProvider, IComponentProvider<Transform, TProvider>;
    }
}