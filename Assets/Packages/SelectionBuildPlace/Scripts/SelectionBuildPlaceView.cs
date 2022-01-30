using System;
using City.GameControl;
using City.Views.Buildings;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace City.Views.SelectionBuildPlace
{
    public class SelectionBuildPlaceView : View
    {
        // x, y
        public Action<int, int> TryBuildAction;
        
        private static float CellWidth = 10;
        private static float CellHeight = 10;
        
        [SerializeField] private GameObject plant;
        
        [SerializeField] private Camera camera;
        [SerializeField] private GameObject _building;
        private GameObject _loadedPrefab;

        private bool _detectionOn;
        
        private int _xCell ;
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
            gameObject.SetActive(false);
            _detectionOn = false;
            RemoveLoadedPrefab();
        }

        public void UpdatePrefab(GameObject prefab)
        {
            RemoveLoadedPrefab();
            _loadedPrefab = Instantiate(prefab, _building.transform, false);
            _loadedPrefab.transform.localPosition = new Vector3(0, _loadedPrefab.transform.position.y, 0);
            
            Building script = _loadedPrefab.GetComponent<Building>();
            script.BuildingSelectedAction += BuildingSelected;
        }

        private void BuildingSelected(BigBuildingModel model)
        {
            TryBuildAction?.Invoke(_xCell, _yCell);
        }
        
        private void RemoveLoadedPrefab()
        {
            if(_loadedPrefab != null)
            {
                Destroy(_loadedPrefab);
            }
        }
        
        void Update()
        {
            if(!_detectionOn || _loadedPrefab == null)
                return;
            
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                if (plant.gameObject == hit.transform.gameObject)
                {
                    _xCell = Mathf.RoundToInt((hit.point.x - 5)/CellWidth);
                    _yCell = Mathf.RoundToInt((hit.point.z + 5)/CellHeight);
                    BuildingSetposition();
                }
                else {
                    Building script = _loadedPrefab.GetComponent<Building>();
                    if(hit.transform.gameObject == script.collider.gameObject)
                    {
                        
                        _xCell = Mathf.RoundToInt((hit.point.x - 5)/CellWidth);
                        _yCell = Mathf.RoundToInt((hit.point.z + 5)/CellHeight);
                        BuildingSetposition();
                    }
                }
            }
        }

        private void BuildingSetposition()
        {
            
            _building.transform.position = new Vector3(
                _xCell * 10, 
                _building.transform.position.y,
                _yCell * 10);
            _loadedPrefab.transform.localPosition = new Vector3(0, _loadedPrefab.transform.position.y, 0);
        }
    }
}