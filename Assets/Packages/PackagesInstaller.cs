using City.Common.ModePanel;
using City.GameControl;
using City.Info.BuildingInfoPopups;
using City.Views.BuildingPlane;
using City.Views.Buildings;
using City.Views.BuildSelectionPanel;
using City.Views.Hud;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using Packages.Navigation;
using Packages.Preloader;
using Packages.SceneController;
using Zenject;

namespace Packages
{
    public class PackagesInstaller: Installer<PackagesInstaller>
    {
        public override void InstallBindings()
        {
            SceneControllerInstaller.Install(Container);
            NavigationInstaller.Install(Container);
            PreloaderInstaller.Install(Container);
            BuildingInfoPopupsInstaller.Install(Container);
            BuildingPlanInstaller.Install(Container);
            BuildingsInstaller.Install(Container);
            BuildingInfoInstaller.Install(Container);
            BuildSelectionPanelInstaller.Install(Container);
            GameControlInstaller.Install(Container);
            HudInstaller.Install(Container);
            ModePanelInstaller.Install(Container);
        }
    }
}