using System;
using System.Collections.Generic;
using City.Common;
using City.Common.Utils;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace City.Views
{
    public class PrefabToLocationService: IPrefabToLocationService, IInitializable
    {
        private IInstantiator _instantiator;
        private IComponentProvisionService _componentProvisionService;

        private Dictionary<Type, PrefabToLocationBinding> _prefabToLocationBindings =
            new Dictionary<Type, PrefabToLocationBinding>();

        private List<IPrefabToLocationContract> _contracts;
        private PrefabsInstaller _prefabsMap;
        
        private Dictionary<Type, GameObject> _prefabs = new Dictionary<Type, GameObject>();
        
        public PrefabToLocationService(IInstantiator instantiator, 
            PrefabsInstaller prefabs,
            List<IPrefabToLocationContract> contracts,
            IComponentProvisionService componentProvisionService)
        {
            _instantiator = instantiator;
            _contracts = contracts;
            _prefabsMap = prefabs;
            _componentProvisionService = componentProvisionService;
        }
        
        public void Initialize()
        {
            BindPrefabsFromFields(_prefabsMap);
            
            foreach (var contract in _contracts)
            {
                var key = contract.PrefabType;    
                
                if(_prefabToLocationBindings.ContainsKey(key))
                    Debug.Log("Multiple PrefabToLocationContracts for {0}" + contract.PrefabType.Name);
                
                var binding = new PrefabToLocationBinding(contract);

                _prefabToLocationBindings[key] = binding;

                var location = GetLocation(contract.LocationProviderType);
                
                if (location == null)
                    continue;
                
                var parent = contract.LocationIsAnchor ? location.parent : location;
                var anchor = contract.LocationIsAnchor ? location : null;
                
                var initiallyOnScene = parent == null 
                        ? GetComponentsOnSceneRootGameObjects(contract.PrefabType)
                        : parent.transform.GetComponentsInFirstLayerChildren(contract.PrefabType);

                if (initiallyOnScene.Length > 1)
                    Debug.Log($"Multiple instances of {contract.PrefabType.Name} are not allowed.");

                for(var i=0;i!=initiallyOnScene.Length;i++)
                    initiallyOnScene[i].gameObject.SetActive(false);

                if (initiallyOnScene.Length <= 0)
                    continue;

                if (anchor != null)
                {
                    var instanceTransform = initiallyOnScene[0].transform;
                    instanceTransform.position = anchor.position;
                    instanceTransform.rotation = anchor.rotation;
                }

                binding.Bind(initiallyOnScene[0]);
                PrepareInstance(binding.Binded);
            }
        }

        public void BindPrefab<TPrefab>(TPrefab prefab) where TPrefab : Component
        {
            BindPrefab(prefab);
        }

        public void BindPrefab(Component prefab)
        {
            Debug.Log($"Prefab of type {prefab.GetType().Name} binded");

            _prefabs[prefab.GetType()] = prefab.gameObject;
        }
        
        public T LoadPrefabByContract<T>() where T : Component
        {
            var binding = GetPrefabToLocationBinding<T>();
            
            if (binding == null)
                return null;

            var preLoaded = binding.Binded;
            
            T instance;

            if (preLoaded == null)
                instance = InstantiatePrefabByContract<T>(binding);
            else
                instance = preLoaded as T;    
            
            return instance;
        }
        
        public void UnloadPrefabByContract<T>() where T : Component
        {
            UnloadPrefabByContract(typeof(T));
        }
        
        private void UnloadPrefabByContract(Type prefabType)
        {
            if (!_prefabToLocationBindings.TryGetValue(prefabType, out var binding))
                Debug.Log("No PrefabToLocationBinding for type " + prefabType.Name);
            
            if(binding == null)
                throw new InvalidOperationException($"Null binding for type{prefabType.Name}");
            
            var contract = binding.Contract;

            var anchor = GetLocation(binding.Contract.LocationProviderType);
            
            if (anchor == null)
                Debug.Log($"{contract.LocationProviderType.Name} binded location must be instantiated before {contract.PrefabType.Name}");
            
            var parent = anchor.parent;  
            
            var instances = parent == null
                ? GetComponentsOnSceneRootGameObjects(contract.PrefabType)
                : parent.GetComponentsInChildren(contract.PrefabType, true);

            Debug.Log(
                $"{instances.Length} instances of {contract.PrefabType.Name} destroyed by contract");

            for (var i = 0; i != instances.Length; i++)
                DestroyInstanceInternal(instances[i], binding);
        }

        public void UnloadAllPrefabsByContract()
        {
            foreach (var binding in _prefabToLocationBindings)
            {
                UnloadPrefabByContract(binding.Value.Contract.PrefabType);
            }
        }
        
        private Component[] GetComponentsOnSceneRootGameObjects(Type componentType)
        {
            var result = new List<Component>();
            
            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var rootObject in rootObjects)
            {
                var components =  rootObject.GetComponents(componentType);

                if(components == null || components.Length == 0)
                    continue;
                
                result.AddRange(components);
            }

            return result.ToArray();
        }
        
        private void DestroyInstanceInternal(Component instance, PrefabToLocationBinding binding)
        {
            binding.Unbind();
            GameObject.Destroy(instance.gameObject);
        }

        private PrefabToLocationBinding GetPrefabToLocationBinding<T>() where T : Component
        {
            if (!_prefabToLocationBindings.TryGetValue(typeof(T), out var binding))
                Debug.Log($"No PrefabToLocationBinding for type " + typeof(T).Name);

            return binding;
        }
        
        private T InstantiatePrefabByContract<T>(PrefabToLocationBinding binding) where T : Component
        {
            var contract = binding.Contract;

            var prefab = GetPrefab(contract.PrefabType);
            
            Debug.Log($"Instance of {contract.PrefabType.Name} created by contract");
            
            var location = GetLocation(contract.LocationProviderType);
            
            if (location == null)
                Debug.Log($"{contract.LocationProviderType.Name} binded location must be instantiated before {contract.PrefabType.Name}");
            
            var instance = InstantiatePrefab<T>(prefab, location, contract.LocationIsAnchor);
            binding.Bind(instance);
            
            return instance;
        }

        private TPrefab InstantiatePrefab<TPrefab>(GameObject prefab, Transform location, bool asAnchor)
            where TPrefab: Component
        {
            var prefabActive = prefab.activeSelf; 
            prefab.SetActive(false);
            
            var instance = asAnchor
                ? _instantiator.InstantiatePrefabForComponent<TPrefab>(prefab, location.position, location.rotation,
                    location.parent)
                : _instantiator.InstantiatePrefabForComponent<TPrefab>(prefab, location);

            prefab.SetActive(prefabActive);
            
            if(instance == null)
                throw new InvalidOperationException($"No {typeof(TPrefab).Name} component on prefab root GameObject");
            
            PrepareInstance(instance);
            
            return instance;
        }
        
        private void PrepareInstance(Component instance)
        {
            var go = instance.gameObject; 
            instance.transform.SetAsLastSibling();
            go.name = instance.GetType().Name;
            
            Debug.Log($"{instance.GetType().Name} prepared");
        }

        private Transform GetLocation(Type locationProviderType)
        {
            return _componentProvisionService.Provide<Transform>(locationProviderType);
        }

        private GameObject GetPrefab(Type prefabType)
        {
            if (_prefabs.TryGetValue(prefabType, out var prefab))
                return prefab;
            
            throw new InvalidOperationException($"No prefab of type {prefabType.Name}");
        }
        
        private void BindPrefabsFromFields(object source)
        {
            var fieldInfos = source.GetType().GetFields();
            foreach (var field in fieldInfos)
            {
                if(!field.IsPublic || !field.FieldType.IsSubclassOf(typeof(Component)))
                    continue;

                var value = field.GetValue(source) as Component;

                if (value == null)
                {
                    Debug.Log($"Prefab of type {field.FieldType.Name} is not set");
                    continue;
                }
                
                BindPrefab(value);
            }
        }
    }
}