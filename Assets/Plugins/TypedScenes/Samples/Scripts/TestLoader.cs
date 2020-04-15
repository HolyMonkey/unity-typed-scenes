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
            TypedScene.LoadScene("3417602231161af48b9682e519590d2f", new ExampleSceneLoadModel("Loading model test"));
        }
    }
}
