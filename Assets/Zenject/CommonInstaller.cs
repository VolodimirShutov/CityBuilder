using City.Views;
using Common.SoundManager;
using Packages;
using Packages.Common.StateMachineGlobal;
using ShootCommon.CachingService;
using ShootCommon.Signals;
using Zenject;

namespace Common
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            CachingServiceInstaller.Install(Container);
            MessageBrokerInstaller.Install(Container);
            SignalBusInstaller.Install(Container);
            SignalsInstaller.Install(Container);
            
            SoundInitstaller.Install(Container);
            PackagesInstaller.Install(Container);

            GlobalStateMachineInstaller.Install(Container);
        }
    }
}