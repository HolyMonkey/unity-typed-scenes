using UnityEngine;

namespace IJunior.TypedScene
{
    public class TestLoader : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                TestScene1.Load();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                TestScene2.Load();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                New_Scene__23.Load();
            }
        }
    } 
}
