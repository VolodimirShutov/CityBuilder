using System.Numerics;
using Packages.CIty.Views.BuildingsInfoView.Scripts;

namespace City.GameControl
{
    public interface IGameControl
    {
        bool CanBuild(BuildingInfoModel building, int xPosition, int yPosition);
        bool OnBuild(BuildingInfoModel building, int xPosition, int yPosition);
        long GetCurrentGold();
        long GetCurrentWood();
        long GetCurrentIron();
        float GetBuildingProductionProgress(int Id);
        bool GetBuildingIsWorking(int Id);
    }
}