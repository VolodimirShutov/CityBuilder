using System;
using System.Collections.Generic;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using UnityEngine;

namespace City.GameControl
{
    public class BuildingsControl
    {
        public Action<int, BuildingInfoModel> ProductionIsFinishedAction;
        
        private List<BuildingControl> _buildings = new List<BuildingControl>();
        private int IdCounter = 0;
        
        public int BuildNew(BuildingInfoModel building)
        {
            IdCounter++;
            BuildingControl newBuilding = new BuildingControl
            {
                Id = IdCounter, 
                BuildingInfo = building
            };
            newBuilding.ProductionIsFinished += ProductionIsFinished;
            newBuilding.Init();
            _buildings.Add(newBuilding);

            return IdCounter;
        }

        private void ProductionIsFinished(BuildingControl building)
        {
            ProductionIsFinishedAction?.Invoke(building.Id, building.BuildingInfo);
        }
        
        private BuildingControl GetBuildingById(int id)
        {
            foreach (BuildingControl building in _buildings)
            {
                if (building.Id == id)
                {
                    return building;
                }
            }
            
            Debug.LogError("Can't find building with Id = " + id);
            return null;
        }

        public BuildingInfoModel GetBuildingInfo(int id)
        {
            BuildingControl building = GetBuildingById(id);
            return building?.BuildingInfo;
        }
        
        public float GetBuildingProductionProgress(int id)
        {
            BuildingControl building = GetBuildingById(id);
            if (building == null)
                return 0;
            return building.ProductionProgress;
        }
        
        public bool GetBuildingIsWorking(int id)
        {
            BuildingControl building = GetBuildingById(id);
            return building != null && building.IsWorking;
        }

        public void OnStartBuildingProduction(int id)
        {
            if(GetBuildingIsWorking(id))
                return;
            
            BuildingControl building = GetBuildingById(id);
            building.OnStartBuildingProduction();
        }
    }
}