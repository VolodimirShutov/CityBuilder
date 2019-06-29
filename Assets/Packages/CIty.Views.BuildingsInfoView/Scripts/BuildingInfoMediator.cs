using City.Views;
using Zenject;

namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    public class BuildingInfoMediator : Mediator <BuildingsInfoView>
    {
        private IBuildingInfoContainer _buildingInfoContainer;
        
        [Inject]
        public void Init(
            IBuildingInfoContainer buildingInfoContainer
        )
        {
            _buildingInfoContainer = buildingInfoContainer;
        }

        protected override void OnMediatorInitialize()
        {
            _buildingInfoContainer.SetBuildings(View.Buildings);
        }
    }
}