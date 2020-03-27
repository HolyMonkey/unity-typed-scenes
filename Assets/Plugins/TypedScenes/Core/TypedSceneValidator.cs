using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;

namespace IJunior.TypedScene
{
    public class TypedSceneValidator
    {
        public static bool ValidateNewScene(string scenePath)
        {
            var name = Path.GetFileNameWithoutExtension(scenePath);
            var validName = GetUniqueName(GetValidName(name));

            if (name != validName)
            {
                var validPath = Path.GetDirectoryName(scenePath) + Path.DirectorySeparatorChar + validName + TypedSceneSettings.SceneExtension;
                File.Move(scenePath, validPath);
                File.Delete(scenePath + TypedSceneSettings.MetaExtension);
                AssetDatabase.ImportAsset(validPath, ImportAssetOptions.ForceUpdate);
                return false;
            }

            return true;
        }

        public static bool SameNameExists(string sceneName)
        {
            var assets = AssetDatabase.FindAssets(sceneName);

            foreach(var asset in assets)
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

        private static string GetUniqueName(string sceneName)
        {
            var derivedClasses = GetDerivedClasses();

            var postfix = "";
            var count = 0;

            while (derivedClasses.Where(type => type.Name == sceneName + postfix).ToArray().Length > 0)
            {
                count++;
                postfix = count.ToString();
            }

            return sceneName + postfix;
        }

        private static IEnumerable<Type> GetDerivedClasses()
        {
            var derivedClasses = new List<Type>();
            foreach (var domainAssembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var assemblyTypes = domainAssembly.GetTypes()
                  .Where(type => type.IsSubclassOf(Type.GetType(TypedSceneSettings.Namespace + "." + TypedSceneSettings.BaseClass)) && !type.IsAbstract);

                derivedClasses.AddRange(assemblyTypes);
            }

            return derivedClasses;
        }
    }
}
