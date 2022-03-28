using Packages.States.GameState;
using Packages.States.InitGameStates;
using ShootCommon.GlobalStateMachine;
using Zenject;

namespace Packages.States
{
    public class StatesInstaller : Installer<StatesInstaller>
    {
        public override void InstallBindings()
        {
            Container.BindState<LoadingSourcesState>();
            Container.BindState<CreateGameSceneState>();
        }
    }
}