using UnityEngine;
using IJunior.TypedScenes;

public class CustomClassHandler : MonoBehaviour, ISceneLoadHandler<CustomClass>
{
    public void OnSceneLoaded(CustomClass argument)
    {
        Debug.Log(argument.ToString());
    }
}
