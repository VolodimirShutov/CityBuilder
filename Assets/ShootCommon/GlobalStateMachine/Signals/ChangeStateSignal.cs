using ShootCommon.Signals;

namespace ShootCommon.GlobalStateMachine.Signals
{
    public class ChangeStateSignal : Signal
    {
        public StateMachineTriggers SelectedState;
    }
}