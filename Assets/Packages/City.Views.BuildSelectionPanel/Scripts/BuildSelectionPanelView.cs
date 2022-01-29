using System;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using ShootCommon.Views.Mediation;
using UnityEngine;

namespace City.Views.BuildSelectionPanel
{
    public class BuildSelectionPanelView : View
    {
        public Action<int> BuildingIsSelectedAction;

        [SerializeField] private GameObject _container;
        [SerializeField] private BuildSelectionButtonView _buttonPrefab;        
        
        public void Show()
        {
            gameObject.SetActive(true);
        }
        
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void CreateNewButton(BuildingInfoModel buildingInfo)
        {
            
            if (_container == null)
            {
                Debug.LogError("Container for Buttons is empty");
                return;
            }
            
            BuildSelectionButtonView button = Instantiate(_buttonPrefab, gameObject.transform, true);
            button.SetButtonInfo(buildingInfo);
            button.BuildingIsSelectedAction += BuildingIsSelected;
            button.gameObject.transform.localScale = new Vector3(1,1,1);
        }

        private void BuildingIsSelected(int value)
        {
            BuildingIsSelectedAction?.Invoke(value);
        }
    }
}