using System;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using UniRx;
using UnityEngine;

namespace City.GameControl
{
    public class BuildingControl
    {
        public static float BuildTime = 10f;
        
        public int Id;
        public BuildingInfoModel BuildingInfo;

        private CompositeDisposable disposables;
        private int _productionProgress = 0;
            
        public float ProductionProgress { get; private set; }
        public bool IsWorking { get; private set; }

        public Action<BuildingControl> ProductionIsFinished;

        public void Init()
        {
            if (BuildingInfo.AutomaticProduction)
                OnStartBuildingProduction();
        }
        
        public void OnStartBuildingProduction()
        {
            if(IsWorking)
                return;
            
            disposables = new CompositeDisposable();
            IsWorking = true;
            _productionProgress = 0;
            
            Observable.Timer (System.TimeSpan.FromSeconds (BuildTime)) 
                .Subscribe (_ => { 
                    FinishProduction();
                }).AddTo (disposables); 

            Observable.Timer (System.TimeSpan.FromSeconds (1)) 
                .Repeat () 
                .Subscribe (_ =>
                {
                    _productionProgress += 1;
                    ProductionProgress = _productionProgress / BuildTime;
                }).AddTo (disposables); 
        }

        private void FinishProduction()
        {
            disposables.Dispose();
            IsWorking = false;
            Debug.Log ("Finish!");
            ProductionIsFinished?.Invoke(this);
            
            if (BuildingInfo.AutomaticProduction)
            {
                OnStartBuildingProduction();
            }
        }
        

        public void Remove () { 
            disposables?.Dispose ();
            ProductionIsFinished = null;
        }
    }
}