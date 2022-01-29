using System;
using City.GameControl;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace City.Views.Buildings
{
    public class BuildingsView : View
    {
        public Action<BigBuildingModel> BuildingSelectedAction;
        
        private static float CellWidth = 10;
        private static float CellHeight = 10;

        public void AddBuilding(BigBuildingModel model)
        {
            GameObject loadedPrefab = Instantiate(model.BuildingInfo.Prefab , transform, false);
            loadedPrefab.transform.localPosition = new Vector3(model.X * CellWidth,
                model.BuildingInfo.Prefab.transform.position.y, 
                model.Y * CellHeight);
            
            
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