using City.Common.ModePanel;
using City.GameControl;
using City.Info.BuildingInfoPopups;
using City.Views.BuildingPlane;
using City.Views.Buildings;
using City.Views.BuildSelectionPanel;
using City.Views.Hud;
using City.Views.SelectionBuildPlace;
using Packages.CIty.Views.BuildingsInfoView.Scripts;
using Zenject;

namespace City.Views
{
    public class ViewsInstaller: Installer<ViewsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<PrefabToLocationService>().AsSingle();
            Container.BindInterfacesTo<ComponentProvisionService>().AsSingle();
            
            GameControlInstaller.Install(Container);
            HudInstaller.Install(Container);
            ModePanelInstaller.Install(Container);
            BuildingInfoInstaller.Install(Container);
            BuildingPlanInstaller.Install(Container);
            BuildSelectionPanelInstaller.Install(Container);
            SelectionBuildPlaceInstaller.Install(Container);
            BuildingsInstaller.Install(Container);
            
            BuildingInfoPopupsInstaller.Install(Container);
        }
    }
}