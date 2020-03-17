using System.IO;
using UnityEditor;
using UnityEngine;

namespace IJunior.TypedScene
{
    public class ScenePostprocessor : AssetPostprocessor
    {
        private const string SceneExtension = ".unity";

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
                if (assetPath.Contains(SceneExtension))
                    TypedSceneManager.Generate(assetPath);
            }
        }

        private static void DetectSceneDeletion(string[] deletedAssets)
        {
            foreach (string assetPath in deletedAssets)
            {
                if (assetPath.Contains(SceneExtension))
                {
                    var sceneName = Path.GetFileNameWithoutExtension(assetPath);
                    TypedSceneManager.Delete(sceneName);
                }
            }
        }

        private static void DetectSceneMovement(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (var i = 0; i < movedFromAssetPaths.Length; i++)
            {
                if (movedFromAssetPaths[i].Contains(SceneExtension))
                {
                    var oldName = Path.GetFileNameWithoutExtension(movedFromAssetPaths[i]);
                    TypedSceneManager.Delete(oldName);

                    if (oldName != Path.GetFileNameWithoutExtension(movedAssets[i]))
                        return;

                    TypedSceneManager.Generate(movedAssets[i]);
                }
            }
        }
    } 
}
