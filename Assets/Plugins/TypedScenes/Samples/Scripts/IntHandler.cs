using IJunior.TypedScenes;
using UnityEngine;

public class IntHandler : MonoBehaviour, ISceneLoadHandler<int>
{
    public void OnSceneLoaded(int argument)
    {
        Debug.Log(argument.ToString());
    }
}
