using City.Views;
using Zenject;

namespace City.Common.ModePanel
{
    public class ModePanelInstaller : Installer<ModePanelInstaller>
    {
        
        public override void InstallBindings()
        {
            Container.BindViewToMediator<ModePanelView, ModePanelMediator>();
            //Container.BindPrefabToRoot<ModePanelView, CanvasHudRoot>();
        }
    }
}