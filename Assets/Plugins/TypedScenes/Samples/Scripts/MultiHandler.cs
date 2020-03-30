using IJunior.TypedScenes;
using UnityEngine;

public class MultiHandler : MonoBehaviour, ISceneLoadHandler<float>, ISceneLoadHandler<bool>
{
    public void OnSceneLoaded(float argument)
    {
        Debug.Log(argument);
    }

    public void OnSceneLoaded(bool argument)
    {
        Debug.Log(argument);
    }
}
