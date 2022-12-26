using System;
using System.Collections.Generic;
using ShootCommon.AssetReferences;
using ShootCommon.InteractiveObjectsSpawnerService.Containers;
using UnityEngine;
using Zenject;

namespace ShootCommon.InteractiveObjectsSpawnerService
{
    public class InteractiveObjectsManager: IInteractiveObjectsManager
    {
        private readonly Dictionary<string, IInteractiveObjectContainer> _containers = new Dictionary<string, IInteractiveObjectContainer>();
        
        private IAssetReferenceDownloader _assetReferenceStorage;

        [Inject]
        public void Init(IAssetReferenceDownloader assetReferenceStorage)
        {
            _assetReferenceStorage = assetReferenceStorage;
        }
        
        public void AddContainer(string key, IInteractiveObjectContainer container)
        {
            if (_containers.ContainsKey(key))
            {
                RemoveContainer(key);
            }
            _containers.Add(key, container);
        }
        
        public void RemoveContainer(string key)
        {
             _containers.Remove(key);
        }

        public bool ContainerIsExists(string containerKey)
        {
            return _containers.ContainsKey(containerKey);
        }

        public IInteractiveObjectContainer GetContainer(string containerKey)
        {
            return _containers.ContainsKey(containerKey)?_containers[containerKey] : null;
        }

        public void Instantiate(string prefabId, string containerKey, Action<GameObject> callback = null, 
            bool inject = true)
        {
            if (!_containers.ContainsKey(containerKey))
            {
                Debug.LogError($"Container {containerKey} don't exist");
                return;
            }
            IInteractiveObjectContainer container = _containers[containerKey];
            Instantiate(prefabId, container, callback, inject);
        }

        public void Instantiate(string prefabId, IInteractiveObjectContainer container,
            Action<GameObject> callback = null, bool inject = true)
        {
            if (container == null)
            {
                Debug.LogError("InteractiveObjectsManager   Instantiate  container is null. ");
                return;
            }

            _assetReferenceStorage.SpawnById(prefabId, go =>
            {
                GameObject item = container.CreateItem(go);
                item.transform.localScale = Vector3.one;
                if (inject)
                    ProjectContext.Instance.Container.InjectGameObject(item);
                callback?.Invoke(item);
            });
        }
    }
}