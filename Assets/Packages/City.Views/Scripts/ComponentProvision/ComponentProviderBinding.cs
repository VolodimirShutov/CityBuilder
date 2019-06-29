using System;
using UnityEngine;
using Zenject;

namespace City.Views
{
    public class ComponentProviderBinding
    {
        public object Id { get; }
        public Type ProvidedType { get; }
        public Func<Component> Provider { get; }
        public ComponentProviderBinding(Type providedType, Func<Component> provider, object id = null)
        {
            ProvidedType = providedType;
            Provider = provider;
            Id = id;
        }
    }
}