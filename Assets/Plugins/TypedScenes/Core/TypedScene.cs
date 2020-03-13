using UnityEngine.SceneManagement;

namespace IJunior.TypedScene
{
    public abstract class TypedScene
    {
        protected static void LoadScene(string path)
        {
            SceneManager.LoadScene(path);
        }
    }
}
