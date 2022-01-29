using Stateless;
using Stateless.Graph;

namespace ShootCommon.GlobalStateMachine
{
    public interface IStateMachineController
    {
        StateMachineTriggers CurrentState { get; }
        void FireState(StateMachineTriggers triggers);
        void Fire(StateMachineTriggers triggers);
        StateMachine<IState, StateMachineTriggers>.StateConfiguration Configure(IState state);
        IState GetState<TState>() where TState: IState;
    }
}