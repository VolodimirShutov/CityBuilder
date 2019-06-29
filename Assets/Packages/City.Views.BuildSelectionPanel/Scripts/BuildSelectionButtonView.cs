using System;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace City.Views.BuildSelectionPanel
{
    public class BuildSelectionButtonView : MonoBehaviour
    {
        public Action<int> BuildingIsSelectedAction;
            
        [SerializeField] private Text _name;
        [SerializeField] private Text _gold;
        [SerializeField] private Text _wood;
        [SerializeField] private Text _iron;

        private BuildingInfoModel _buildingInfo;

        public void SetButtonInfo(BuildingInfoModel buildingInfo)
        {
            _buildingInfo = buildingInfo;

            if(_name != null)
                _name.text = buildingInfo.Name;
            if(_gold != null)
                _gold.text = buildingInfo.PriceGold.ToString();
            if(_wood != null)
                _wood.text = buildingInfo.PriceWood.ToString();
            if(_iron != null)
                _iron.text = buildingInfo.PriceIron.ToString();
        }

        public void OnButtonClick()
        {
            BuildingIsSelectedAction?.Invoke(_buildingInfo.Id);
        }
    }
}