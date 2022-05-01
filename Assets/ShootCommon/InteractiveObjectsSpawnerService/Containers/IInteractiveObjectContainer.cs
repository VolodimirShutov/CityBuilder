using UnityEngine;

namespace ShootCommon.InteractiveObjectsSpawnerService.Containers
{
    public interface IInteractiveObjectContainer
    {
        GameObject CreateItem(GameObject inst);
        bool ContainerIsExist();
    }
}