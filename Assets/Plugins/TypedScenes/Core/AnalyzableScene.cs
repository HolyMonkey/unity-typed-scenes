#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class AnalyzableScene : IDisposable
    {
        private bool _closeOnDispose;
        
        public Scene Scene { get; private set; }
        public string Name { get; private set; }
        public string GUID { get; private set; }
        public string AssetPath { get; private set; }
        
        private AnalyzableScene() {}

        public static AnalyzableScene Create(string scenePath)
        {
            var scene = SceneManager.GetActiveScene();
            var sceneIsActive = scene.path == scenePath;

            if (!sceneIsActive)
            {
                scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                EditorSceneManager.SetActiveScene(scene);
            }
            
            var guid = AssetDatabase.AssetPathToGUID(scene.path);
            var name = Path.GetFileNameWithoutExtension(scenePath);

            return new AnalyzableScene
            {
                _closeOnDispose = !sceneIsActive,
                Name = name,
                Scene = scene,
                GUID = guid,
                AssetPath = scene.path
            };
        }

        public void Dispose()
        {
            if (!_closeOnDispose) return;
            EditorSceneManager.CloseScene(Scene, true);
        }
    }
}
#endif
