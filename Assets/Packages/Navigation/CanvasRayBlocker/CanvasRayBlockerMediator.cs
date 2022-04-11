using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;

namespace Packages.Navigation.CanvasRayBlocker
{
    public class CanvasRayBlockerMediator : Mediator<CanvasRayBlockerView>
    {
        protected override void OnMediatorInitialize()
        {
            View.EnterAction += OnEnterAction;
            View.ExitAction += OnExitAction;
        }

        private void OnEnterAction()
        {
            SignalService.Publish(new CanvasRayBlockSignal()
            {
                BlockKey = View.keyName
            });
        }

        private void OnExitAction()
        {
            SignalService.Publish(new CanvasRayUnblockSignal()
            {
                BlockKey = View.keyName
            });
        }

        protected override void OnMediatorDispose()
        {
            OnExitAction();
            base.OnMediatorDispose();
        }
    }
}