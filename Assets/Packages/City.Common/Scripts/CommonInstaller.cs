using City.Common.Signals;
using City.Views;
using Zenject;


namespace City.Common
{
    public class CommonInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            MessageBrokerInstaller.Install(Container);
            SignalBusInstaller.Install(Container);
            
            SignalsInstaller.Install(Container);
            
            ViewsInstaller.Install(Container);
            
        }
    }
}