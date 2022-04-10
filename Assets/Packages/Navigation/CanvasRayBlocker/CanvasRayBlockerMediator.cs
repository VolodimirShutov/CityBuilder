using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;
using UnityEngine;

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
            Debug.Log("OnEnterAction ");
            SignalService.Publish(new CanvasRayBlockSignal()
            {
                BlockKey = View.keyName
            });
        }

        private void OnExitAction()
        {
            Debug.Log("OnExitAction ");
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