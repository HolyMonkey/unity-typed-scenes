using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class TypedProcessor : MonoBehaviour
    {
        private void Awake()
        {
            foreach(var rootObject in SceneManager.GetActiveScene().GetRootGameObjects())
            {
                foreach (var handler in rootObject.GetComponentsInChildren<ITypedAwakeHandler>())
                {
                    handler.OnSceneAwake();
                }
            }
        }
    }
}
