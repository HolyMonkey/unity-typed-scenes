using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public abstract class TypedScene
    {
        protected static void LoadScene(string sceneName, LoadSceneMode loadSceneMode)
        {
            Action<AsyncOperation> handler = null;
            handler = asyncOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                asyncOperation.completed -= handler;
            };

            var loader = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            loader.completed += handler;
        }

        protected static void LoadScene<T>(string sceneName, LoadSceneMode loadSceneMode, T argument)
        {
            Action<AsyncOperation> handler = null;
            handler = asyncOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                HandleSceneLoaders(argument);
                asyncOperation.completed -= handler;
            };

            var loader = SceneManager.LoadSceneAsync(sceneName, loadSceneMode);
            loader.completed += handler;
        }

        private static void HandleSceneLoaders<T>(T loadingModel)
        {
            foreach (var rootObjects in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (var handler in rootObjects.GetComponentsInChildren<ISceneLoadHandler<T>>())
                {
                    handler.OnSceneLoaded(loadingModel);
                }
            }
        }
    }
}
