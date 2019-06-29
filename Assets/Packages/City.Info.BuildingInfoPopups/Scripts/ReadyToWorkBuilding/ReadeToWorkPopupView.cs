using City.GameControl;

namespace City.Info.BuildingInfoPopups
{
    public class ReadeToWorkPopupView : BuildingPopupBase
    {
        
        public void StartProduce()
        {
            SignalService.Publish(new OnStartBuildingProductionSignal()
            {
                Value = Model.Id
            });
            Finish();
        }
    }
}