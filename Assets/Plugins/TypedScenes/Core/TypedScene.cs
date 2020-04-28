using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public abstract class TypedScene
    {
        protected static void LoadScene(string guid, LoadSceneMode loadSceneMode)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            Action<AsyncOperation> handler = null;
            handler = asyncOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
                asyncOperation.completed -= handler;
            };

            var loader = SceneManager.LoadSceneAsync(path, loadSceneMode);
            loader.completed += handler;
        }

        protected static void LoadScene<T>(string guid, LoadSceneMode loadSceneMode, T argument)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            Action<AsyncOperation> handler = null;
            handler = asyncOperation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByPath(path));
                HandleSceneLoaders(argument);
                asyncOperation.completed -= handler;
            };

            var loader = SceneManager.LoadSceneAsync(path, loadSceneMode);
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
