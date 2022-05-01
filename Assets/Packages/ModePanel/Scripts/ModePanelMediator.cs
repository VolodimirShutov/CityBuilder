using ShootCommon.CachingService;
using ShootCommon.Signals;
using ShootCommon.Views.Mediation;
using UnityEngine;
using Zenject;

namespace City.Common.ModePanel
{
    public class ModePanelMediator : Mediator<ModePanelView>
    {
        private ICachingService _cachingService;
        
        [Inject]
        public void Init(ICachingService cachingService)
        {
            _cachingService = cachingService;
        }
        
        protected override void OnMediatorInitialize()
        {
            View.RegularMode += RegularModeSelected;
            View.BuildMode += BuildModeSelected;
        }

        private int counter;
        private void RegularModeSelected()
        {
            _cachingService.Read<TestModel>("test", mode =>
            {
                if(mode == null)
                    return;
                Debug.Log(mode.Number);
                Debug.Log(mode.Text); 
            });

            counter++;
            TestModel model = new TestModel()
            {
                Text = "text " + counter,
                Number = counter
            };
            
            _cachingService.Save<TestModel>("test", model, () => { });
            SignalService.Publish(new RegularModeSelected());
        }

        private void BuildModeSelected()
        {
            SignalService.Publish(new BuildModeSelected());
        }
    }

    public class TestModel
    {
        public string Text;
        public int Number;
    }
    
    public class RegularModeSelected : AsyncSignal{}
    public class BuildModeSelected : AsyncSignal{}
}