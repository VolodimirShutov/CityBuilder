using City.Common.ModePanel;
using City.GameControl;
using City.Views;
using City.Views.Buildings;
using City.Views.BuildSelectionPanel;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Views.Mediation;
using UniRx;
using Zenject;

namespace City.Info.BuildingInfoPopups
{
    public class BuildingInfoPopupsMediator : Mediator<BuildingInfoPopupsView>
    {
        private  IGameControl _gameControl;
        
        [Inject]
        public void Init( IGameControl gameControl )
        {
            _gameControl = gameControl;
        }
        
        protected override void OnMediatorInitialize()
        {
            SignalService.Receive<RegularModeSelected>()
                .Subscribe(RegularMode).AddTo(DisposeOnDestroy);
            SignalService.Receive<BuildModeSelected>()
                .Subscribe(BuildMode).AddTo(DisposeOnDestroy);
            
            SignalService.Receive<SowBuildingInfoSignal>()
                .Subscribe(SowBuildingInfo).AddTo(DisposeOnDestroy);

        }

        public void SowBuildingInfo(SowBuildingInfoSignal signal)
        {
            BigBuildingModel model = signal.Value;
            
            bool buildingIsWorking = _gameControl.GetBuildingIsWorking(model.Id);
            
            View.CreateNewPopup(signal.Value, buildingIsWorking, SignalService, _gameControl);
        }
        
        private void RegularMode(RegularModeSelected signal= null)
        {
        }
        
        private void BuildMode(BuildModeSelected signal = null)
        {
        }


    }
}