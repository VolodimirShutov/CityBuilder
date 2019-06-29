namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    public class BuildingInfoContainer : IBuildingInfoContainer
    {
        private BuildingInfoModel[] _buildings;


        public void SetBuildings(BuildingInfoModel[] value)
        {
            _buildings = value;
        }

        public void AddBuilding(BuildingInfoModel value)
        {
            //todo
        }

        public BuildingInfoModel GetBuildingById(int value)
        {
            return _buildings[value];
        }

        public int GetBuildingsCount()
        {
            return _buildings.Length;
        }
    }
}