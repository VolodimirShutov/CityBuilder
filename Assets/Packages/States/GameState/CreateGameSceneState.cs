using Packages.SceneController;
using ShootCommon.GlobalStateMachine;
using Zenject;

namespace Packages.States.GameState
{
    public class CreateGameSceneState : GlobalState
    {
        private ISceneController _sceneController;

        protected override void Configure()
        {
            //Permit<LoadingSourcesState>(StateMachineTriggers.LoadingSourcesState);
        }

        [Inject]
        public void Init(ISceneController sceneController)
        {
            _sceneController = sceneController;
        }

        protected override void OnEntry()
        {
            _sceneController.LoadScene(SceneController.SceneController.CityScene);
            //Fire(StateMachineTriggers.LoadingSourcesState);
        }
    }
}