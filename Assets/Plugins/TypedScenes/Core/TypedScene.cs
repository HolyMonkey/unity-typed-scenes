using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScene
{
    public class TypedScene : MonoBehaviour
    {
        protected const string _path = "";

        public void LoadScene()
        {
            SceneManager.LoadScene(_path);
        }
    }
}
