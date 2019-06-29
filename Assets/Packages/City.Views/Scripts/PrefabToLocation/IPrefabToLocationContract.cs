using System;
using UnityEngine;

namespace City.Views
{
    public interface IPrefabToLocationContract
    {
        Type LocationProviderType { get; }
        Type PrefabType { get; }
        bool LocationIsAnchor { get; }
    }
}