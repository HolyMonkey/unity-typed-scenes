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
                    Debug.Log("Сцена добавлена");
            }
        }

        private static void DetectSceneDeletion(string[] deletedAssets)
        {
            foreach (string assetPath in deletedAssets)
            {
                if (assetPath.Contains(SceneExtension))
                    Debug.Log("Сцена удалена");
            }
        }

        private static void DetectSceneMovement(string[] movedAssets, string[] movedFromAssetPaths)
        {
            for (var i = 0; i < movedFromAssetPaths.Length; i++)
            {
                if (movedFromAssetPaths[i].Contains(SceneExtension))
                {
                    if (Path.GetFileNameWithoutExtension(movedFromAssetPaths[i]) != Path.GetFileNameWithoutExtension(movedAssets[i]))
                    {
                        Debug.Log("Сцена переименована");
                        return;
                    }
                    Debug.Log("Сцена перемещена");
                }
            }
        }
    } 
}
