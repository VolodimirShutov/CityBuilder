using UnityEngine;

namespace City.Views
{
    public interface IPrefabToLocationService
    {
        void BindPrefab<TPrefab>(TPrefab prefab) where TPrefab : Component;
        void BindPrefab(Component prefab);
        TPrefab LoadPrefabByContract<TPrefab>() where TPrefab : Component;
        void UnloadPrefabByContract<TPrefab>() where TPrefab : Component;
        void UnloadAllPrefabsByContract();
    }
}