using System.IO;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public abstract class TypedScene
    {
        protected static void LoadScene(string guid, LoadSceneMode loadSceneMode)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            SceneManager.LoadScene(path, loadSceneMode);
        }

        protected static void LoadScene<T>(string guid, LoadSceneMode loadSceneMode, T argument)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            UnityAction<Scene, Scene> handler = null;
            handler = (from, to) =>
            {
                if (to.name == Path.GetFileNameWithoutExtension(path))
                {
                    SceneManager.activeSceneChanged -= handler;
                    HandleSceneLoaders(argument);
                }
            };

            SceneManager.activeSceneChanged += handler;
            SceneManager.LoadScene(path, loadSceneMode);
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
