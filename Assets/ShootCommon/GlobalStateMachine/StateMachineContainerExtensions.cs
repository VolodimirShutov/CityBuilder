using Zenject;

namespace ShootCommon.GlobalStateMachine
{
    public static class StateMachineContainerExtensions
    {
        public static void BindState<TState>(this DiContainer container)
            where TState : IState
        {
            container.BindInterfacesAndSelfTo<TState>().AsSingle();
        }
    }
}