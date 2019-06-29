using UnityEngine;
using Zenject;

namespace City.Views
{
    public static class PrefabToLocationContainerExtensions
    {
        public static void BindPrefabToAnchor<TPrefab, TAnchor>(this DiContainer container) 
            where TPrefab: Component
            where TAnchor: class, ILocationProvider 
        {
            container.AddPrefabToLocationContract(
                new PrefabToLocationContract(typeof(TPrefab), typeof(TAnchor), true));
        }
        
        public static void BindPrefabToRoot<TPrefab, TRoot>(this DiContainer container) 
            where TPrefab: Component
            where TRoot: class, ILocationProvider 
        {
            container.AddPrefabToLocationContract(
                new PrefabToLocationContract(typeof(TPrefab), typeof(TRoot), false));
        }
        
        public static void AddPrefabToLocationContract(this DiContainer container, IPrefabToLocationContract contract)
        {
            Debug.Log($"PrefabToLocation contract for {contract.PrefabType.Name} to anchor with id {contract.LocationProviderType.Name} created");
            container.QueueForInject(contract);
            container.Bind<IPrefabToLocationContract>().FromInstance(contract).AsTransient();
        }
            
        
    }
}