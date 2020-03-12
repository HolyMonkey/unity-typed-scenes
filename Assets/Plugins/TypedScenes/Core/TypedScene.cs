using UnityEngine.SceneManagement;

namespace IJunior.TypedScene
{
    public class TypedScene
    {
        protected const string _path = "";

        public void LoadScene()
        {
            SceneManager.LoadScene(_path);
        }
    }
}
