using Packages.States.GameState;
using ShootCommon.GlobalStateMachine;

namespace Packages.States.InitGameStates
{
    public class LoadingSourcesState : GlobalState
    {
        protected override void Configure()
        {
            Permit<CreateGameSceneState>(StateMachineTriggers.CreateGameSceneState);
        }

        protected override void OnEntry()
        {
            Fire(StateMachineTriggers.CreateGameSceneState);
        }
    }
}