using System;
using City.GameControl;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace City.Views.Buildings
{
    public class BuildingsView : View
    {
        public Action<BigBuildingModel> BuildingSelectedAction;
        
        public void AddBuilding(BigBuildingModel model)
        {
            GameObject loadedPrefab = Instantiate(model.BuildingInfo.Prefab , transform, false);
            loadedPrefab.transform.localPosition = new Vector3(model.X , 0, model.Y);
            
            Building script = loadedPrefab.GetComponent<Building>();
            script.bigBuildingModel = model;
            script.BuildingSelectedAction += BuildingSelected;
        }

        private void BuildingSelected(BigBuildingModel model)
        {
            BuildingSelectedAction?.Invoke(model);
        }
    }
}