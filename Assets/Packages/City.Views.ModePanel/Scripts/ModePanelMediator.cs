using City.Common.Signals;
using City.Views;

namespace City.Common.ModePanel
{
    public class ModePanelMediator : Mediator<ModePanelView>
    {
        protected override void OnMediatorInitialize()
        {
            View.RegularMode += RegularModeSelected;
            View.BuildMode += BuildModeSelected;
        }

        private void RegularModeSelected()
        {
            SignalService.Publish(new RegularModeSelected());
        }

        private void BuildModeSelected()
        {
            SignalService.Publish(new BuildModeSelected());
        }
    }
    
    public class RegularModeSelected : AsyncSignal{}
    public class BuildModeSelected : AsyncSignal{}
}