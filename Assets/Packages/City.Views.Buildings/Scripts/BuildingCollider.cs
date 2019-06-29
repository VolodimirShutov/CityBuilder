using System;
using UnityEngine;

namespace City.Views.Buildings
{
    public class BuildingCollider : MonoBehaviour
    {
        public Action OnCickAction;

        void OnMouseDown()
        {
            OnCickAction?.Invoke();
        }
    }
}