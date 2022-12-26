using ShootCommon.InteractiveObjectsSpawnerService.Containers;
using UnityEngine;

namespace ShootCommon.InteractiveObjectsSpawnerService.Tests.tests.Mocks
{
    public class InteractiveObjectContainerMock : IInteractiveObjectContainer
    {
        public GameObject CreateItem(GameObject inst)
        {
            return inst;
        }

        public bool ContainerIsExist()
        {
            return true;
        }

        public void AddItem(GameObject item)
        {
            
        }
    }
}