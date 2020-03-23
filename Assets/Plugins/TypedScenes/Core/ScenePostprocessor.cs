using System.IO;
using UnityEditor;

namespace IJunior.TypedScene
{
    public class ScenePostprocessor : AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            DetectSceneCreation(importedAssets);
            DetectSceneDeletion(deletedAssets);
            DetectSceneMovement(movedAssets, movedFromAssetPaths);
        }

        private static void DetectSceneCreation(string[] importedAssets)
        {
            foreach (string assetPath in importedAssets)
            {
                if (Path.GetExtension(assetPath) == TypedSceneSettings.SceneExtension
                    && TypedSceneValidator.ValidateNewScene(assetPath, out var className, out var GUID))
                {
                    var sourceCode = TypedSceneGenerator.Generate(className, GUID);
                    TypedSceneStorage.Save(className, sourceCode);
                }
            }
        }

        private static void DetectSceneDeletion(string[] deletedAssets)
        {
            foreach (string assetPath in deletedAssets)
            {
                if (Path.GetExtension(assetPath) == TypedSceneSettings.SceneExtension)
                {
                    var className = TypedSceneValidator.GetValidName(Path.GetFileNameWithoutExtension(assetPath));
                    TypedSceneStorage.Delete(className);
                }
            }
        }

        private static void DetectSceneMovement(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (var i = 0; i < movedFromAssetPaths.Length; i++)
            {
                if (Path.GetExtension(movedFromAssetPaths[i]) == TypedSceneSettings.SceneExtension)
                {
                    var oldClassName = Path.GetFileNameWithoutExtension(movedFromAssetPaths[i]);
                    TypedSceneStorage.Delete(oldClassName);

                    if (oldClassName != Path.GetFileNameWithoutExtension(movedAssets[i])
                        && TypedSceneValidator.ValidateNewScene(movedAssets[i], out var className, out var GUID))
                    {
                        var sourceCode = TypedSceneGenerator.Generate(className, GUID);
                        TypedSceneStorage.Save(className, sourceCode);
                    }

                    //TypedSceneManager.Generate(movedAssets[i]);
                }
            }
        }
    }
}
