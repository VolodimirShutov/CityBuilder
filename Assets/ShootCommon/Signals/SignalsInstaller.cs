using Zenject;

namespace ShootCommon.Signals
{
    public class SignalsInstaller: Installer<SignalsInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<SignalService>().AsSingle();
        }
    }
}