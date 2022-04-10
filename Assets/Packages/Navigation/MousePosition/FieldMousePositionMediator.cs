using System;
using System.Collections.Generic;
using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;
using UniRx;

namespace Packages.Navigation.MousePosition
{
    public class FieldMousePositionMediator: Mediator<FieldMousePositionView>
    {
        private List<String> _blockers = new List<string>();
        
        protected override void OnMediatorInitialize()
        {
            base.OnMediatorInitialize();
            View.UpdateMousePositionAction += UpdateMousePosition;
            View.OnMouseDown += OnMouseDown;
            View.OnMouseUp += OnMouseUp;
            View.OnMouseClick += OnMouseClick;

            SignalService.Receive<CanvasRayBlockSignal>().Subscribe(CanvasRayBlock).AddTo(DisposeOnDestroy);
            SignalService.Receive<CanvasRayUnblockSignal>().Subscribe(CanvasRayUnblock).AddTo(DisposeOnDestroy);
        }

        private void UpdateMousePosition(UpdateMousePositionSignal signal)
        {
            //Vector3 position = signal.Ray.GetPoint(signal.Distance);
            //Debug.Log(position);
            SignalService.Publish(signal);
        }

        private void OnMouseDown(DownMouseSignal mouseSignal)
        {
            SignalService.Publish(mouseSignal);
        }

        private void OnMouseUp(UpMouseSignal mouseSignal)
        {
            SignalService.Publish(mouseSignal);
        }

        private void OnMouseClick(ClickMouseSignal clickMouseSignal)
        {
            SignalService.Publish(clickMouseSignal);
        }
        
        private void CanvasRayBlock(CanvasRayBlockSignal signal)
        {
            if(!_blockers.Contains(signal.BlockKey))
                _blockers.Add(signal.BlockKey);
            View.GameStopped = true;
        }

        private void CanvasRayUnblock(CanvasRayUnblockSignal signal)
        {
            if(_blockers.Contains(signal.BlockKey))
                _blockers.Remove(signal.BlockKey);
            if (_blockers.Count == 0)
                View.GameStopped = false;
        }
    }
}