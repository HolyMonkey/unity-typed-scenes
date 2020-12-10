using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class LoadingProcessor : MonoBehaviour
    {
        private static LoadingProcessor _instance;
        private Action _loadingModelAction;

        public static LoadingProcessor Instance
        {
            get
            {
                if (_instance == null)
                    Initialize();

                return _instance;
            }
        }

        private static void Initialize()
        {
            _instance = new GameObject("TypedProcessor").AddComponent<LoadingProcessor>();
            _instance.transform.SetParent(null);
            DontDestroyOnLoad(_instance);
        }

        public void ApplyLoadingModel()
        {
            _loadingModelAction?.Invoke();
            _loadingModelAction = null;
        }

        public void RegisterLoadingModel<T>(T loadingModel)
        {
            _loadingModelAction = () =>
            {
                foreach (var rootObjects in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    foreach (var handler in rootObjects.GetComponentsInChildren<ISceneLoadHandler<T>>())
                    {
                        handler.OnSceneLoaded(loadingModel);
                    }
                }
            };
        }
    }
}
