using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;


namespace Common
{
    public class LoadMainScene : MonoBehaviour
    {
    
        [FormerlySerializedAs("_mainSceneIndex")] [SerializeField]
        private int mainSceneIndex = 1;
        
        void Start()
        {
            Observable.NextFrame()
                .Do(_ => SceneManager.LoadScene(mainSceneIndex))
                .Subscribe();
        }
    }
}