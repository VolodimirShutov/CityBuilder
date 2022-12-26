using UnityEngine;

namespace ShootCommon.InteractiveObjectsSpawnerService.Containers
{
    public interface IInteractiveObjectContainer
    {
        public GameObject CreateItem(GameObject inst);
        public bool ContainerIsExist();
        public void AddItem(GameObject item);
    }
}