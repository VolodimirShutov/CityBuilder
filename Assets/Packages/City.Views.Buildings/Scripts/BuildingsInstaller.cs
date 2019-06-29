using Zenject;

namespace City.Views.Buildings
{
    public class BuildingsInstaller : Installer<BuildingsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<BuildingsView, BuildingsMediator>();
        }
        
    }
}