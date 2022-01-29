using ShootCommon.Views.Mediation;
using Zenject;

namespace Packages.CIty.Views.BuildingsInfoView.Scripts
{
    public class BuildingInfoInstaller : Installer<BuildingInfoInstaller>
    {
        
        public override void InstallBindings()
        {
            Container.Bind<IBuildingInfoContainer>().To<BuildingInfoContainer>().AsSingle();
            Container.BindViewToMediator<BuildingsInfoView, BuildingInfoMediator>();
        }
    }
}