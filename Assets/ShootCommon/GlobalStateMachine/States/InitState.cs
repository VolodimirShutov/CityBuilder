namespace ShootCommon.GlobalStateMachine.States
{
    public class InitState : GlobalState
    {
        protected override void Configure()
        {
            //Permit<StartState>(StateMachineTriggers.Start);
        }
    }
}