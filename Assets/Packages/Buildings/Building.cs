using System;
using City.GameControl;
using UniRx;
using UnityEngine;
using UnityEngine.Serialization;

namespace City.Views.Buildings
{
    public class Building : MonoBehaviour
    {
        public Action<BigBuildingModel> BuildingSelectedAction;
        [FormerlySerializedAs("TryBuildModel")] public BigBuildingModel bigBuildingModel;
        [SerializeField] public BuildingCollider collider;
        [SerializeField] private string buildingId = "";
        
        private void Start()
        {
            collider.OnCickAction += BuildingSelected;
        }

        private void BuildingSelected()
        {
            if(bigBuildingModel != null)
                bigBuildingModel.BuildingGameObject = gameObject;
            BuildingSelectedAction?.Invoke(bigBuildingModel);
        }
    }

}