#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;

namespace IJunior.TypedScenes
{
    public class ScenePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            foreach (var importedAsset in importedAssets)
            {
                if (TypedSceneValidator.DetectSceneImport(importedAsset, out var validScenePath))
                    HandleImportedScene(validScenePath);
            }
            
            foreach (var deletedAsset in deletedAssets)
            {
                if (Path.GetExtension(deletedAsset) == TypedSceneSettings.SceneExtension
                    && !TypedSceneValidator.DetectSceneDeletion(Path.GetFileNameWithoutExtension(deletedAsset)))
                    HandleSceneDeletion(deletedAsset);
            }
            
            for (var i = 0; i < movedFromAssetPaths.Length; i++)
            {
                if (Path.GetExtension(movedFromAssetPaths[i]) == TypedSceneSettings.SceneExtension)
                    HandleSceneMovement(movedFromAssetPaths[i], movedAssets[i]);
            }
        }

        private static void HandleImportedScene(string scenePath)
        {
            using (var analyzableScene = AnalyzableScene.Create(scenePath))
            {
                var sourceCode = TypedSceneGenerator.Generate(analyzableScene);
                TypedSceneStorage.Save(analyzableScene.Name, sourceCode);
            
                if (EditorBuildSettings.scenes.All(scene => scene.guid.ToString() != analyzableScene.GUID))
                {
                    var buildScenes = EditorBuildSettings.scenes;
                    Array.Resize(ref buildScenes, buildScenes.Length + 1);
                    buildScenes[buildScenes.Length - 1] = new EditorBuildSettingsScene(analyzableScene.AssetPath, true);
                    EditorBuildSettings.scenes = buildScenes;
                }
            }
        }

        private static void HandleSceneDeletion(string scenePath)
        {
            var newSceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
            newSceneList.RemoveAll(scene => scene.path == scenePath);
            EditorBuildSettings.scenes = newSceneList.ToArray();
            TypedSceneStorage.Delete(Path.GetFileNameWithoutExtension(scenePath));
        }

        private static void HandleSceneMovement(string oldPath, string newPath)
        {
            var oldName = Path.GetFileNameWithoutExtension(oldPath);
            var newName = Path.GetFileNameWithoutExtension(newPath);

            if (oldName != newName)
                TypedSceneStorage.Delete(oldName);
        }
    }
}
#endif
