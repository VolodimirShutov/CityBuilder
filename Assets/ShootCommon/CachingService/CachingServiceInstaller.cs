using SQLite4Unity3d;
using Zenject;

namespace ShootCommon.CachingService
{
    public class CachingServiceInstaller : Installer<CachingServiceInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SaveManager>().AsSingle();
            Container.BindInterfacesTo<CachingServiceSQL>().AsSingle();
        }
    }
}