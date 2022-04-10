using City.Common.ModePanel;
using Packages.BuildSelectionPanel.Signals;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Views.Mediation;
using UniRx;
using Zenject;

namespace City.Views.BuildSelectionPanel
{
    public class BuildSelectionPanelMediator : Mediator<BuildSelectionPanelView>
    {
        private IBuildingInfoContainer _buildingInfoContainer;
        
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
            
            RegularMode();
            
            
            Observable.Timer (System.TimeSpan.FromSeconds (0.5)) 
                .Subscribe (_ => { 
                    CreteBuildingsButtons();
                }).AddTo (DisposeOnDestroy);

            View.BuildingIsSelectedAction += BuildingIsSelected;
        }

        private void CreteBuildingsButtons()
        {
            int count =_buildingInfoContainer.GetBuildingsCount();
            for (int i = 0; i < count; i++)
            {
                BuildingInfoModel buildingInfo = _buildingInfoContainer.GetBuildingById(i);
                View.CreateNewButton(buildingInfo);
            }
        }
        
        private void RegularMode(RegularModeSelected signal= null)
        {
            View.Hide();
        }
        
        private void BuildMode(BuildModeSelected signal = null)
        {
            View.Show();
        }

        private void BuildingIsSelected(int id)
        {
            SignalService.Publish(new BuildBuildingIsSelectedSignal()
            {
                Value = id
            });
        }
    }
}