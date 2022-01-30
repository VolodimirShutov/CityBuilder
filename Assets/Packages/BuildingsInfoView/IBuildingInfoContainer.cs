namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    public interface IBuildingInfoContainer
    {
        void SetBuildings(BuildingInfoModel[] value);
        void AddBuilding(BuildingInfoModel value);
        BuildingInfoModel GetBuildingById(int value);
        int GetBuildingsCount();
        
    }
}