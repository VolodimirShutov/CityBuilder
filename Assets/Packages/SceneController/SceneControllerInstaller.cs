using Zenject;

namespace Packages.SceneController
{
    public class SceneControllerInstaller : Installer<SceneControllerInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SceneController>().AsSingle();
        }
    }
}