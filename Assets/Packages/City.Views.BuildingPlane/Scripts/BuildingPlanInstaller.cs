using ShootCommon.Views.Mediation;
using Zenject;

namespace City.Views.BuildingPlane
{
    public class BuildingPlanInstaller : Installer<BuildingPlanInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<BuildingPlanView, BuildingPlanMediator>();
        }
    }
}