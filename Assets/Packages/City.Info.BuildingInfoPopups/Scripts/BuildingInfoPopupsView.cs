using City.Common.Signals;
using City.GameControl;
using City.Views;
using City.Views.Buildings;
using UnityEngine;

namespace City.Info.BuildingInfoPopups
{
    public class BuildingInfoPopupsView : View
    {
        [SerializeField] private Camera _camera;
        [SerializeField] private Canvas parentCanvas;
        
        [SerializeField] private BuildingPopupBase _simplePopup;
        [SerializeField] private BuildingPopupBase _readyPopup;
        [SerializeField] private BuildingPopupBase _workinPopup;

        public void CreateNewPopup(
            BigBuildingModel model,
            bool buildingIsWorking,
            ISignalService signalService,
            IGameControl gameControl )
        {
            BuildingPopupBase prefab = _simplePopup;
            if (model.BuildingInfo.GoldProduction == 0 &&
                model.BuildingInfo.WoodProduction == 0 &&
                model.BuildingInfo.IronProduction == 0)
            {
                
                prefab = _simplePopup;
            }
            else if(buildingIsWorking)
            {
                prefab = _workinPopup;
            }
            else
            {
                prefab = _readyPopup;
            }
            
            BuildingPopupBase newPopup = Instantiate(prefab, transform, false);
            
            Vector3 screenPos = _camera.WorldToScreenPoint(model.BuildingGameObject.transform.position);
            Vector2 movePos;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
            Vector3 viewPos =  parentCanvas.transform.TransformPoint(movePos);
            
            newPopup.transform.localPosition = movePos;
            
            newPopup.SetInfo(model);
            newPopup.SignalService = signalService;
            newPopup.GameControl = gameControl;
        }
        
        
    }
}