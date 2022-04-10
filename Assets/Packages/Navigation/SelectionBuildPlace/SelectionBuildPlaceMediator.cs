using City.Common.ModePanel;
using City.GameControl;
using Packages.BuildSelectionPanel.Signals;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;
using UniRx;
using UnityEngine;
using Zenject;

namespace Packages.Navigation.SelectionBuildPlace
{
    public  class SelectionBuildPlaceMediator : Mediator<SelectionBuildPlaceView>
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
            
            SignalService.Receive<UpdateMousePositionSignal>().Subscribe(UpdateMousePosition).AddTo(DisposeOnDestroy);
            SignalService.Receive<ClickMouseSignal>().Subscribe(ClickMouse).AddTo(DisposeOnDestroy);
        }

        private void UpdateMousePosition(UpdateMousePositionSignal signal)
        {
            View.NewMousePosition(signal);
        }

        private void ClickMouse(ClickMouseSignal signal)
        {
            Vector3 position = signal.Ray.GetPoint(signal.Distance);
            TryBuild(position);
        }

        private void TryBuild(Vector3 position)
        {
            if(_buildingInfo == null)
                return;
            
            int xCell = Mathf.RoundToInt(position.x);
            int yCell = Mathf.RoundToInt(position.z);
            
            Debug.Log(xCell + "   " + yCell);
            
            BigBuildingModel model = new BigBuildingModel()
            {
                X = xCell,
                Y = yCell,
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