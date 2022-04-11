using City.Common.ModePanel;
using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;
using UniRx;
using UnityEngine;

namespace Packages.Navigation.CameraControl
{
    public class CameraControlMediator: Mediator<CameraControlView>
    {
        private bool _mouseIsDown;
        private bool _dragActive;
        
        protected override void OnMediatorInitialize()
        {
            SignalService.Receive<RegularModeSelected>().Subscribe(RegularMode).AddTo(DisposeOnDestroy);
            SignalService.Receive<BuildModeSelected>().Subscribe(BuildMode).AddTo(DisposeOnDestroy);
            
            SignalService.Receive<UpdateMousePositionSignal>().Subscribe(UpdateMousePosition).AddTo(DisposeOnDestroy);
            SignalService.Receive<DownMouseSignal>().Subscribe(DownMouse).AddTo(DisposeOnDestroy);
            SignalService.Receive<UpMouseSignal>().Subscribe(UpMouse).AddTo(DisposeOnDestroy);
            SignalService.Receive<DiscardMoveSignal>().Subscribe(DiscardMove).AddTo(DisposeOnDestroy);
        }

        
        private void RegularMode(RegularModeSelected signal= null)
        {
            _dragActive = true;
        }
        
        private void BuildMode(BuildModeSelected signal = null)
        {
            _dragActive = false;
        }

        private Vector2 _position;
        private Vector2 _lastMovePosition;
        private void UpdateMousePosition(UpdateMousePositionSignal signal)
        {
            if (_mouseIsDown && _dragActive)
            {
                
                Vector2 changeMove = signal.FingerPosition - _lastMovePosition;
                _position -= changeMove * View.speed;
                _lastMovePosition = signal.FingerPosition;
                
                Debug.Log(signal.FingerPosition + "      " + changeMove + "     " +  _position);
                View.SetPosition(_position);
            }
        }

        private void DownMouse(DownMouseSignal signal)
        {
            _lastMovePosition = signal.FingerPosition;
            _mouseIsDown = true;
        }

        private void UpMouse(UpMouseSignal signal)
        {
            _mouseIsDown = false;
        }

        private void DiscardMove(DiscardMoveSignal signal)
        {
            _mouseIsDown = false;
        }
    }
}