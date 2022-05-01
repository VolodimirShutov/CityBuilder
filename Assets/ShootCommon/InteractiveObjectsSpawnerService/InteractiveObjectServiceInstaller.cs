using Zenject;

namespace ShootCommon.InteractiveObjectsSpawnerService
{
    public class InteractiveObjectServiceInstaller : Installer<InteractiveObjectServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<InteractiveObjectsManager>().AsSingle();
        }
    }
}