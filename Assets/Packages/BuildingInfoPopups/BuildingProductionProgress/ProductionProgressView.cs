using City.GameControl;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace City.Info.BuildingInfoPopups
{
    public class ProductionProgressView : BuildingPopupBase
    {
        [SerializeField] private Slider ProgressBar;
        
        private void Start()
        {
            UpdateProgress();
            Observable.Timer (System.TimeSpan.FromSeconds (1)) 
                .Repeat () 
                .Subscribe (_ =>
                {
                    UpdateProgress();
                }).AddTo (Disposables); 
        }

        private void UpdateProgress()
        {
            if (GameControl != null)
            {
                ProgressBar.value = GameControl.GetBuildingProductionProgress(Model.Id);
            }
        }
    }
}