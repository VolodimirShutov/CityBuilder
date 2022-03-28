using Packages.SceneController;
using Packages.States.InitGameStates;
using Zenject;

namespace ShootCommon.GlobalStateMachine.States
{
    public class StartState: GlobalState
    {
        private ISceneController _sceneController;
        protected override void Configure()
        {
            Permit<LoadingSourcesState>(StateMachineTriggers.LoadingSourcesState);
        }

        [Inject]
        public void Init(ISceneController sceneController)
        {
            _sceneController = sceneController;
        }
        
        protected override void OnEntry()
        {
            _sceneController.LoadScene(SceneController.PreloaderScene);
            Fire(StateMachineTriggers.LoadingSourcesState);
        }
    }
}