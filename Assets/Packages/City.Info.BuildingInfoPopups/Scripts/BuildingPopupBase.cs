using City.Common.Signals;
using City.GameControl;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace City.Info.BuildingInfoPopups
{
    public class BuildingPopupBase: MonoBehaviour, IBuildingPopupView
    {
        [SerializeField] private Text _name;
        public ISignalService SignalService { set; get; }
        public IGameControl GameControl { set; get; }
        
        protected CompositeDisposable Disposables = new CompositeDisposable();
        protected BigBuildingModel Model;
        
        public void SetInfo(BigBuildingModel model)
        {
            Model = model;
            _name.text = model.BuildingInfo.Name;
        }

        private void Awake()
        {
            Observable.Timer (System.TimeSpan.FromSeconds(5)) 
                .Subscribe (_ => { 
                    Finish();
                }).AddTo (Disposables); 
        }

        protected void Finish()
        {            
            Disposables.Dispose();
            Destroy(gameObject);
        }

        public virtual void SetProgress(int progress)
        {
            
        }
        
    }
}