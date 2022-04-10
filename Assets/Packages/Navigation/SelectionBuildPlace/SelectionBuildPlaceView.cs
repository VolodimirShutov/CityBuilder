using System;
using City.GameControl;
using Packages.Navigation.Signals;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace Packages.Navigation.SelectionBuildPlace
{
    public class SelectionBuildPlaceView : View
    {

        [SerializeField] private GameObject _building;
        private GameObject _loadedPrefab;

        private bool _detectionOn;
        
        private int _xCell;
        private int _yCell;
        
        public void Show()
        {
            gameObject.SetActive(true);
            _detectionOn = true;
            /*
            this.OnPointerClickAsObservable()
                .Subscribe(x => { }, error => { })
                .AddTo(_disposable);
                */
        }

        public void Hide()
        {
            _detectionOn = false;
            RemoveLoadedPrefab();
        }

        public void UpdatePrefab(GameObject prefab)
        {
            RemoveLoadedPrefab();
            _loadedPrefab = Instantiate(prefab, _building.transform, false);
            _loadedPrefab.transform.localPosition = new Vector3(0, 0, 0);
            
            //Building script = _loadedPrefab.GetComponent<Building>();
            //script.BuildingSelectedAction += BuildingSelected;
        }

        private void RemoveLoadedPrefab()
        {
            if(_loadedPrefab != null)
            {
                Destroy(_loadedPrefab);
            }
        }
        
        public void NewMousePosition(UpdateMousePositionSignal signal)
        {
            if(!_detectionOn || _loadedPrefab == null)
                return;
            
            Vector3 position = signal.Ray.GetPoint(signal.Distance);
            _xCell = Mathf.RoundToInt(position.x);
            _yCell = Mathf.RoundToInt(position.z);
            BuildingSetposition();
        }

        private void BuildingSetposition()
        {
            _building.transform.localPosition = new Vector3(_xCell, 0, _yCell);
            _loadedPrefab.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}