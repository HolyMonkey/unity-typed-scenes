using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace IJunior.TypedScene
{
    public class TypedSceneValidator
    {
        public static bool ValidateNewScene(string scenePath)
        {
            var name = Path.GetFileNameWithoutExtension(scenePath);
            var validName = GetUniqueSceneName(GetValidName(name));

            if (name != validName)
            {
                AssetDatabase.RenameAsset(scenePath, validName);
                return false;
            }

            return true;
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

        private static string GetUniqueSceneName(string sceneName)
        {
            var postfix = "";
            var count = 0;

            while (Type.GetType(sceneName + postfix) != null)
            {
                count++;
                postfix = count.ToString();
            }

            return sceneName;
        }
    }
}
