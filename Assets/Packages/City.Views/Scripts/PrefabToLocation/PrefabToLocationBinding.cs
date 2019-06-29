using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

namespace City.Views
{
    public class PrefabToLocationBinding
    {
        private Component _binded;
        
        public IPrefabToLocationContract Contract { get; }

        public Component Binded => _binded;
        public bool IsBinded => _binded != null;

        public PrefabToLocationBinding(IPrefabToLocationContract contract)
        {
            Contract = contract;
        }
        
        public void Bind(Component instance)
        {
            if (_binded != null)
            {
                Debug.Log($"{instance.GetType().Name} already binded ");
                return;
            }

            _binded = instance;
        }
        
        public void Unbind() => _binded = null;
    }
}