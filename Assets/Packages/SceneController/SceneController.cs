using UnityEngine.SceneManagement;

namespace Packages.SceneController
{
    public class SceneController : ISceneController
    {
        public static string PreloaderScene = "PreloaderScene";
        public static string CityScene = "CityScene";
        
        public void LoadScene(string sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}