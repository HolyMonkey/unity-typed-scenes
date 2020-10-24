#if UNITY_EDITOR
using System.IO;
using UnityEditor;

namespace IJunior.TypedScenes
{
    public class TypedSceneStorage
    {
        public static void Save(string className, string sourceCode)
        {
            var path = TypedSceneSettings.SavingDirectory + className + TypedSceneSettings.ClassExtension;
            Directory.CreateDirectory(TypedSceneSettings.SavingDirectory);

            if (File.Exists(path) && File.ReadAllText(path) == sourceCode)
                return;

            File.WriteAllText(path, sourceCode);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        public static void Delete(string className)
        {
            var path = TypedSceneSettings.SavingDirectory + className + TypedSceneSettings.ClassExtension;

            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }
    }
}
#endif
