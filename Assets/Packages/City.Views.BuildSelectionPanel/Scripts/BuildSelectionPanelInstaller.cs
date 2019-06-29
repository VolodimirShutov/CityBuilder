using Zenject;

namespace City.Views.BuildSelectionPanel
{
    public class BuildSelectionPanelInstaller : Installer<BuildSelectionPanelInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<BuildSelectionPanelView, BuildSelectionPanelMediator>();
        }
    }
}
