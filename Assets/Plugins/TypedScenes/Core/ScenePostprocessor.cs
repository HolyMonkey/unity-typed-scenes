using System.IO;
using UnityEditor;

namespace IJunior.TypedScenes
{
    public class ScenePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            DetectSceneCreation(importedAssets);
            //DetectSceneDeletion(deletedAssets);
            //DetectSceneMovement(movedAssets, movedFromAssetPaths);
        }

        private static void DetectSceneCreation(string[] importedAssets)
        {
            foreach (string assetPath in importedAssets)
            {
                if (Path.GetExtension(assetPath) == TypedSceneSettings.SceneExtension
                    && TypedSceneValidator.ValidateNewScene(assetPath))
                {
                    var name = Path.GetFileNameWithoutExtension(assetPath);
                    var guid = AssetDatabase.AssetPathToGUID(assetPath);
                    var sourceCode = TypedSceneGenerator.Generate(name, guid);
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
                    TypedSceneStorage.Delete(Path.GetFileNameWithoutExtension(assetPath));
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
