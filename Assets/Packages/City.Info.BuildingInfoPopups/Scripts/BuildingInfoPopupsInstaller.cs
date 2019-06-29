using City.Views;
using Zenject;

namespace City.Info.BuildingInfoPopups
{
    public class BuildingInfoPopupsInstaller : Installer<BuildingInfoPopupsInstaller>
    {
        
        public override void InstallBindings()
        {
            Container.BindViewToMediator<BuildingInfoPopupsView, BuildingInfoPopupsMediator>();
        }

    }
}