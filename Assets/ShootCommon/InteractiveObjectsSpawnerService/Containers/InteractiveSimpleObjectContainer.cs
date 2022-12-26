using UnityEngine;

namespace ShootCommon.InteractiveObjectsSpawnerService.Containers
{
    public class InteractiveSimpleObjectContainer: MonoBehaviour, IInteractiveObjectContainer
    {
        public GameObject CreateItem(GameObject inst)
        {
            GameObject item = Instantiate(inst, transform, false);
            return item;
        }

        public bool ContainerIsExist()
        {
            return gameObject != null;
        }

        public void AddItem(GameObject item)
        {
            item.transform.parent = transform;
        }
    }
}