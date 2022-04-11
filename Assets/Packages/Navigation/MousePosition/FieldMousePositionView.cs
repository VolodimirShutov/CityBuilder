using System;
using System.Collections.Generic;
using System.Linq;
using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace Packages.Navigation.MousePosition
{
    public class FieldMousePositionView : View
    {
        public Action<UpdateMousePositionSignal> UpdateMousePositionAction { get; set; }
        public Action<DownMouseSignal> OnMouseDown { get; set; }
        public Action<UpMouseSignal> OnMouseUp { get; set; }
        public Action<ClickMouseSignal> OnMouseClick { get; set; }
        public Action<DiscardMoveSignal> DiscardMove { get; set; }
        public bool GameStopped { get; set; } = false;
        private List<ActiveToucheModel> _activeTouches = new List<ActiveToucheModel>();
        private Camera _mainCamera;
        private readonly float _clickDistance = 1f;
        
        public void Start()
        {
            //ProjectContext.Instance.Container.Inject(this);
            _mainCamera = Camera.main;
        }

        private ActiveToucheModel GetActiveTouchesById(int id)
        {
            foreach (ActiveToucheModel touch in _activeTouches)
            {
                if (touch.ID == id)
                    return touch;
            }
            return null;
        }
        
        public void FixedUpdate()
        {
            if (GameStopped)
            {
                foreach (ActiveToucheModel model in _activeTouches)
                {
                    DiscardMove?.Invoke(new DiscardMoveSignal()
                    {
                        Id = model.ID
                    });
                }

                _activeTouches = new List<ActiveToucheModel>();
                return;
            }
#if UNITY_EDITOR
            EditorInput();
#else
            DeviceInput();
#endif
        }
        
        private void EditorInput()
        {
            if (Input.GetMouseButton(0))
            {
                UpdateButtonInfoById(0, Input.mousePosition);
            }
            else
            {
                ActiveToucheModel toucheModel = GetActiveTouchesById(0);
                MouseUp(toucheModel, Input.mousePosition);
            }

            UpdatePosition(0, Input.mousePosition);
        }
        
        private void DeviceInput()
        {
            Touch[] touches = Input.touches;
            foreach (Touch touche in touches)
            {
                UpdateButtonInfoById(touche.fingerId, touche.position);
                UpdatePosition(touche.fingerId, touche.position);
            }

            foreach (var touche in _activeTouches.Where(touche => !CheckTouchWithId(touches, touche.ID)))
            {
                MouseUp(new ActiveToucheModel()
                {
                    ID = touche.ID
                }, touche.StartPosition);
            }
        }
        
        private void UpdateButtonInfoById(int fingerId, Vector2 position)
        {
            bool exist = GetActiveTouchesById(fingerId) != null;

            if (!exist)
            {
                MouseDown(fingerId, position);
            }
        }

        private void UpdatePosition(int fingerId, Vector2 position)
        {
            Vector3 mousePoint = position;
            Ray ray = _mainCamera.ScreenPointToRay(mousePoint);

            Plane plane = new Plane(Vector3.up, 0f);
            plane.Raycast(ray, out var distance);

            UpdateMousePositionAction?.Invoke(new UpdateMousePositionSignal()
            {
                Distance = distance,
                Ray = ray,
                TouchId = fingerId,
                FingerPosition = position
            });
        }

        private void MouseDown(int fingerId, Vector2 position)
        {
            _activeTouches.Add(new ActiveToucheModel()
            {
                ID = fingerId,
                StartPosition = position
            });
            OnMouseDown?.Invoke(new DownMouseSignal()
            {
                TouchId = fingerId,
                FingerPosition = position
            });
        }

        private void MouseUp(ActiveToucheModel touche, Vector2 position)
        {
            if(touche == null)
                return;
            _activeTouches.Remove(touche);
            OnMouseUp?.Invoke(new UpMouseSignal()
            {
                TouchId = touche.ID
            });

            if (!CheckClick(touche, position)) return;
                
            Vector3 mousePoint = position;
            Ray ray = _mainCamera.ScreenPointToRay(mousePoint);
            Plane plane = new Plane(Vector3.up, 0f);
            plane.Raycast(ray, out var distance);
                
            OnMouseClick?.Invoke(new ClickMouseSignal()
            {
                TouchId = touche.ID,
                Distance = distance,
                Ray = ray
            });
        }

        private bool CheckClick(ActiveToucheModel touche, Vector2 position)
        {
            return (Vector2.Distance(touche.StartPosition, position) < _clickDistance);
        }

        private bool CheckTouchWithId(Touch[] touches, int id)
        {
            foreach (Touch touch in touches)
            {
                if (touch.fingerId == id)
                    return true;
            }
            return false;
        }

        private class ActiveToucheModel
        {
            public int ID;
            public Vector2 StartPosition;
        }
    }
}