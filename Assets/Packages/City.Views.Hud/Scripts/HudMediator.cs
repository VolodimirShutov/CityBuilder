using City.GameControl;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Views.Mediation;
using UniRx;
using Zenject;

namespace City.Views.Hud
{
    public class HudMediator : Mediator<HudView>
    {
        [Inject]
        public void Init(
            IGameControl gameControl
        )
        {
            View.UpdateGold(gameControl.GetCurrentGold());
            View.UpdateWood(gameControl.GetCurrentWood());
            View.UpdateIron(gameControl.GetCurrentIron());
        }
        
        protected override void OnMediatorInitialize()
        {
            SignalService.Receive<UpdateGoldSignal>()
                .Subscribe(UpdateGold).AddTo(DisposeOnDestroy);
            SignalService.Receive<UpdateWoodSignal>()
                .Subscribe(UpdateWood).AddTo(DisposeOnDestroy);
            SignalService.Receive<UpdateIronSignal>()
                .Subscribe(UpdateIron).AddTo(DisposeOnDestroy);
        }

        private void UpdateGold(UpdateGoldSignal signal)
        {
            View.UpdateGold(signal.Value);
        }
        private void UpdateWood(UpdateWoodSignal signal)
        {
            View.UpdateWood(signal.Value);
        }
        private void UpdateIron(UpdateIronSignal signal)
        {
            View.UpdateIron(signal.Value);
        }
    }
}