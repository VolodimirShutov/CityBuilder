using ShootCommon.Views.Mediation;
using Zenject;

namespace City.Views.SelectionBuildPlace
{
    public class SelectionBuildPlaceInstaller : Installer<SelectionBuildPlaceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<SelectionBuildPlaceView, SelectionBuildPlaceMediator>();
        }
    }
}