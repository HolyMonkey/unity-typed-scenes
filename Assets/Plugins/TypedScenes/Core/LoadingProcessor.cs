using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class LoadingProcessor : MonoBehaviour
    {
        private static LoadingProcessor _instance;
        private LoadingSettings _loadingSettings;

        public static LoadingProcessor Instance
        {
            get
            {
                if (_instance == null)
                    Initialize();

                return _instance;
            }
        }

        public LoadingSettings LoadingSettings
        {
            get
            {
                if (_loadingSettings == null)
                    _loadingSettings = LoadingSettings.Create();

                return _loadingSettings;
            }
        }

        private static void Initialize()
        {
            _instance = new GameObject("LoadingProcessor").AddComponent<LoadingProcessor>();
            _instance.transform.SetParent(null);
            DontDestroyOnLoad(_instance);
        }

        public void ReloadScene(bool saveLoadingModel = true)
        {
            if (saveLoadingModel == false)
                LoadingSettings.Clear();
            
            var activeScene = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene(activeScene);
        }

        public AsyncOperation ReloadSceneAsync(bool saveLoadingModel = true)
        {
            if (saveLoadingModel == false)
                LoadingSettings.Clear();
            
            var activeScene = SceneManager.GetActiveScene().name;
            return SceneManager.LoadSceneAsync(activeScene);
        }
    }
}
