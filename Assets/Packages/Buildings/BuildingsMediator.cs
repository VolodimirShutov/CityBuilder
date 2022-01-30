using City.Common.ModePanel;
using City.GameControl;
using ShootCommon.Signals;
using ShootCommon.Views.Mediation;
using UniRx;

namespace City.Views.Buildings
{
    public class BuildingsMediator : Mediator <BuildingsView>
    {

        private bool _regularMode;
        
        protected override void OnMediatorInitialize()
        {
            SignalService.Receive<RegularModeSelected>()
                .Subscribe(RegularMode).AddTo(DisposeOnDestroy);
            SignalService.Receive<BuildModeSelected>()
                .Subscribe(BuildMode).AddTo(DisposeOnDestroy);
            _regularMode = true;
            
            
            SignalService.Receive<CreateBuildingViewSignal>()
                .Subscribe(CreateBuildingView).AddTo(DisposeOnDestroy);

            View.BuildingSelectedAction += BuildingSelected;
        }

        private void BuildingSelected(BigBuildingModel model)
        {
            SignalService.Publish(new SowBuildingInfoSignal()
            {
                Value = model
            });
        }
        
        private void RegularMode(RegularModeSelected signal= null)
        {
            _regularMode = true;
        }
        
        private void BuildMode(BuildModeSelected signal = null)
        {
            _regularMode = false;
        }

        private void CreateBuildingView(CreateBuildingViewSignal signal)
        {
            BigBuildingModel model = signal.Value;
            View.AddBuilding(model);
        }

    }
    
    public class SowBuildingInfoSignal : AsyncSignal<BigBuildingModel>{}
}