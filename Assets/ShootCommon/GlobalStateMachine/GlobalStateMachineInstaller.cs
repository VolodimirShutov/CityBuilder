using ShootCommon.GlobalStateMachine;
using ShootCommon.GlobalStateMachine.States;
using Zenject;

namespace Packages.Common.StateMachineGlobal
{
    public class GlobalStateMachineInstaller : Installer<GlobalStateMachineInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindState<InitState>();
            Container.BindInterfacesTo<StateMachineController>().AsSingle();
        }
    }
}