using System.Linq;
using UnityEngine;

namespace IJunior.TypedScenes
{
    public class TypedProcessor : MonoBehaviour
    {
        private void Awake()
        {
            foreach(var handler in FindObjectsOfType<MonoBehaviour>().OfType<ITypedAwakeHandler>())
            {
                handler.OnSceneAwake();
            }
            
            LoadingProcessor.Instance.LoadingSettings.Apply();
        }
    }
}
