using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Utility to be able to create a assets-only entry scene with minimal code.
/// That way, the Unity launch screen is removed earlier and the user see visual feedback about the app loading.
/// Tip: Use the same visual prefab in the entry and main scene to get a seamless experience
/// Unity behaviour:
///     - If the entry scene contain Bootstrapping code (Zenject installers, Kochava, music, etc), any time that this code take on the Awake phase will be added to the Unity launch screen
///     - This will not decrease loading time, but the time took by the code mentioned in the previous point will be done after the use see the entry assets, improving loading perceived speed
/// </summary>
namespace City.Common
{
    public class LoadMainScene : MonoBehaviour
    {
    
        [SerializeField]
        private int _mainSceneIndex = 1;
        
        void Start()
        {
            Observable.NextFrame()
                .Do(_ => SceneManager.LoadScene(_mainSceneIndex))
                .Subscribe();
        }
    }
}