using UnityEditor;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScene
{
    public abstract class TypedScene
    {
        protected static void LoadScene(string guid)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            SceneManager.LoadScene(path);
        }
    }
}
