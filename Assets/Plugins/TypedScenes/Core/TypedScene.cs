using System.IO;
using UnityEditor;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public abstract class TypedScene
    {
        protected static void LoadScene(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            SceneManager.LoadScene(path);
        }

        protected static void LoadScene<T>(string guid, T argument)
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
            SceneManager.LoadScene(path);
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
