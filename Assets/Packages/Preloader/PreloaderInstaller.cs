using ShootCommon.Views.Mediation;
using Zenject;

namespace Packages.Preloader
{
    public class PreloaderInstaller : Installer<PreloaderInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindViewToMediator<PreloaderView, PreloaderMediator>();
        }
    }
}