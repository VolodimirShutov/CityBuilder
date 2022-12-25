using System.Collections;
using NUnit.Framework;
using ShootCommon.InteractiveObjectsSpawnerService;
using UnityEngine.TestTools;

public class InteractiveObjectsManagerTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void InteractiveObjectsManagerTestSimplePasses()
    {
    }

    [Test]
    public void CheckAddItem()
    {
        
    }

    [UnityTest]
    public IEnumerator InteractiveObjectsManagerTestWithEnumeratorPasses()
    {
        yield return null;
    }

    private InteractiveObjectsManager InitManager()
    {
        InteractiveObjectsManager manager = new InteractiveObjectsManager();
        //manager.Init();
        return manager;
    }
    
    
    /*
     * 
        void AddContainer(string key, IInteractiveObjectContainer container);
        void RemoveContainer(string key);
        void Instantiate(string prefabId, string containerKey, Action<GameObject> callback = null,
            bool inject = true);
        void Instantiate(string prefabId, IInteractiveObjectContainer container, Action<GameObject> callback = null,
            bool inject = true);
        bool ContainerIsExists(string containerKey);
     */
}
