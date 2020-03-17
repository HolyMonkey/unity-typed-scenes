namespace IJunior.TypedScene
{
    using UnityEditor;
    using UnityEngine;

    public class SceneCreationTrigger : AssetModificationProcessor
    {
        private const string SceneExtension = ".unity";
        private const string MetaExtension = ".meta";

        private static void OnWillCreateAsset(string assetName)
        {
            assetName = assetName.Replace(MetaExtension, "");

            //if (assetName.Contains(SceneExtension))
                //TypedSceneGenerator.Generate(assetName);
        }
    }


}
