using City.GameControl;

namespace City.Info.BuildingInfoPopups
{
    public interface IBuildingPopupView
    {
        void SetInfo(BigBuildingModel model);
        void SetProgress(int progress);
    }
}