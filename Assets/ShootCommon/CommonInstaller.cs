using City.Views;
using Packages.Common.StateMachineGlobal;
using ShootCommon.Signals;
using Zenject;

namespace Common
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            MessageBrokerInstaller.Install(Container);
            SignalBusInstaller.Install(Container);
            SignalsInstaller.Install(Container);

            GlobalStateMachineInstaller.Install(Container);
        }
    }
}