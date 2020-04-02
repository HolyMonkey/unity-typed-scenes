using UnityEngine;
using IJunior.TypedScenes;

public class CustomClassHandler : MonoBehaviour, ISceneLoadHandler<ExampleSceneLoadModel>
{
    public void OnSceneLoaded(ExampleSceneLoadModel argument)
    {
        Debug.Log(argument.ToString());
    }
}
