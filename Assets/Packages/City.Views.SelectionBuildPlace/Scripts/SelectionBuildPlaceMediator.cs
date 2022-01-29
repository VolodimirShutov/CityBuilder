using City.Common.ModePanel;
using City.GameControl;
using City.Views.BuildSelectionPanel;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Views.Mediation;
using UniRx;
using Zenject;

namespace City.Views.SelectionBuildPlace
{
    public class SelectionBuildPlaceMediator : Mediator<SelectionBuildPlaceView>
    {
        private IBuildingInfoContainer _buildingInfoContainer;
        private BuildingInfoModel _buildingInfo;
        
        [Inject]
        public void Init(
            IBuildingInfoContainer buildingInfoContainer)
        {
            _buildingInfoContainer = buildingInfoContainer;
        }
        
        protected override void OnMediatorInitialize()
        {
            SignalService.Receive<RegularModeSelected>()
                .Subscribe(RegularMode).AddTo(DisposeOnDestroy);
            SignalService.Receive<BuildModeSelected>()
                .Subscribe(BuildMode).AddTo(DisposeOnDestroy);
            
            SignalService.Receive<BuildBuildingIsSelectedSignal>()
                .Subscribe(BuildBuildingIsSelected).AddTo(DisposeOnDestroy);

            View.TryBuildAction += TryBuild;
        }

        private void TryBuild(int x, int y)
        {
            if(_buildingInfo == null)
                return;
            
            BigBuildingModel model = new BigBuildingModel()
            {
                X = x,
                Y = y,
                BuildingInfo = _buildingInfo
            };
            SignalService.Publish(new TryBuildSignal()
            {
                Value = model
            });
        }
        
        private void RegularMode(RegularModeSelected signal= null)
        {
            View.Hide();
            _buildingInfo = null;
        }
        
        private void BuildMode(BuildModeSelected signal = null)
        {
            View.Show();
        }

        private void BuildBuildingIsSelected(BuildBuildingIsSelectedSignal signal)
        {
            int id = signal.Value;
            _buildingInfo = _buildingInfoContainer.GetBuildingById(id);
            View.UpdatePrefab(_buildingInfo.Prefab);
        }

    }
}