using Zenject;

namespace Common
{
    public class PrefabsInstaller : MonoInstaller<PrefabsInstaller>
    {
        public override void InstallBindings()
        {
            //Container.Bind<PrefabsInstaller>().FromInstance(this);    
        }
    }
}