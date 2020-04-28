using System.IO;
using System.Text;
using UnityEditor;

namespace IJunior.TypedScenes
{
    public class TypedSceneValidator
    {
        public static bool ValidateSceneImport(string scenePath)
        {
            var name = Path.GetFileNameWithoutExtension(scenePath);
            var validName = GetValidName(name);

            if (name != validName)
            {
                var validPath = Path.GetDirectoryName(scenePath) + Path.DirectorySeparatorChar + validName + TypedSceneSettings.SceneExtension;
                File.Move(scenePath, validPath);
                File.Delete(scenePath + TypedSceneSettings.MetaExtension);
                AssetDatabase.ImportAsset(validPath, ImportAssetOptions.ForceUpdate);
                return false;
            }

            if (SceneAnalyzer.TryAddTypedProcessor(AssetDatabase.AssetPathToGUID(scenePath)))
                return false;

            return true;
        }

        public static bool ValidateSceneDeletion(string sceneName)
        {
            var assets = AssetDatabase.FindAssets(sceneName);

            foreach (var asset in assets)
            {
                var path = AssetDatabase.GUIDToAssetPath(asset);
                var name = Path.GetFileNameWithoutExtension(path);

                if (name != sceneName)
                    continue;

                if (Path.GetExtension(path) == TypedSceneSettings.SceneExtension)
                    return true;
            }

            return false;
        }

        private static string GetValidName(string sceneName)
        {
            var stringBuilder = new StringBuilder();

            if (!char.IsLetter(sceneName[0]) && sceneName[0] != '_')
                stringBuilder.Append('_');

            foreach (var symbol in sceneName)
            {
                stringBuilder.Append((char.IsLetterOrDigit(symbol) || symbol == '_') ? symbol : '_');
            }

            return stringBuilder.ToString();
        }
    }
}
