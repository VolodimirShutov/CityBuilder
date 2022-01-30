using City.Common.ModePanel;
using ShootCommon.Views.Mediation;
using UniRx;

namespace City.Views.BuildingPlane
{
    public class BuildingPlanMediator : Mediator<BuildingPlanView>
    {
        protected override void OnMediatorInitialize()
        {
            SignalService.Receive<RegularModeSelected>()
                .Subscribe(RegularMode).AddTo(DisposeOnDestroy);
            SignalService.Receive<BuildModeSelected>()
                .Subscribe(BuildMode).AddTo(DisposeOnDestroy);
            RegularMode();
        }

        private void RegularMode(RegularModeSelected signal= null)
        {
            View.Hide();
        }
        
        private void BuildMode(BuildModeSelected signal = null)
        {
            View.Show();
        }

    }
}