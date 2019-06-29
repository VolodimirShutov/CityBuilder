using System;
using UnityEngine;

namespace City.Views
{
    public class PrefabToLocationContract: IPrefabToLocationContract
    {
        public Type LocationProviderType { get; }
        public Type PrefabType { get; }

        public bool LocationIsAnchor { get; }

        public PrefabToLocationContract(Type prefabType, Type locationProviderType, bool locationIsAnchor)
        {
            PrefabType = prefabType;
            LocationProviderType = locationProviderType;
            LocationIsAnchor = locationIsAnchor;
        }
    }
}