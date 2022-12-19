using Zenject;

namespace Common.SoundManager
{
    public class SoundInitstaller : Installer<SoundInitstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SoundManager>().AsSingle();
        }
    }
}