using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IJunior.TypedScenes;

public class TestLoader : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SampleScene.Load();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            New_Scene.Load(new ExampleSceneLoadModel("Loading model test"));
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            New_Scene.Load(32);
        }
    }
}
