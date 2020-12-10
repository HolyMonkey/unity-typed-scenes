#if UNITY_EDITOR
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
            DetectSceneImport(importedAssets);
            DetectSceneDeletion(deletedAssets);
            DetectSceneMovement(movedAssets, movedFromAssetPaths);
        }

        private static void DetectSceneImport(string[] importedAssets)
        {
            foreach (string assetPath in importedAssets)
            {
                if (Path.GetExtension(assetPath) == TypedSceneSettings.SceneExtension
                    && TypedSceneValidator.ValidateSceneImport(assetPath))
                {
                    var name = Path.GetFileNameWithoutExtension(assetPath);
                    var sourceCode = TypedSceneGenerator.Generate(name, name, AssetDatabase.AssetPathToGUID(assetPath));
                    var guid = AssetDatabase.AssetPathToGUID(assetPath);
                    if (!EditorBuildSettings.scenes.Any(scene => scene.guid.ToString() == guid))
                    {
                        var newSceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                        newSceneList.Add(new EditorBuildSettingsScene(assetPath, true));
                        EditorBuildSettings.scenes = newSceneList.ToArray();
                    }
                    TypedSceneStorage.Save(name, sourceCode);
                }
            }
        }

        private static void DetectSceneDeletion(string[] deletedAssets)
        {
            foreach (string assetPath in deletedAssets)
            {
                if (Path.GetExtension(assetPath) == TypedSceneSettings.SceneExtension
                    && !TypedSceneValidator.ValidateSceneDeletion(Path.GetFileNameWithoutExtension(assetPath)))
                {
                    var newSceneList = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
                    newSceneList.RemoveAll(scene => scene.path == assetPath);
                    EditorBuildSettings.scenes = newSceneList.ToArray();
                    TypedSceneStorage.Delete(Path.GetFileNameWithoutExtension(assetPath));
                }
            }
        }

        private static void DetectSceneMovement(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (var i = 0; i < movedFromAssetPaths.Length; i++)
            {
                if (Path.GetExtension(movedFromAssetPaths[i]) == TypedSceneSettings.SceneExtension)
                {
                    var oldName = Path.GetFileNameWithoutExtension(movedFromAssetPaths[i]);
                    var newName = Path.GetFileNameWithoutExtension(movedAssets[i]);

                    if (oldName != newName)
                        TypedSceneStorage.Delete(oldName);
                }
            }
        }
    }
}
#endif
