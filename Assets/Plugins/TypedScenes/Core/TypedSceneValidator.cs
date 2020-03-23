using System;
using System.IO;
using System.Text;
using UnityEditor;

namespace IJunior.TypedScene
{
    public class TypedSceneValidator
    {
        public static bool ValidateNewScene(string scenePath, out string className, out string GUID)
        {
            className = "";
            GUID = "";

            var sceneName = Path.GetFileNameWithoutExtension(scenePath);
            var validName = GetUniqueSceneName(GetValidName(sceneName));

            if (sceneName != validName)
            {
                AssetDatabase.RenameAsset(scenePath, validName);
                return false;
            }

            className = validName;
            GUID = AssetDatabase.AssetPathToGUID(scenePath);

            return true;
        }

        public static string GetValidName(string sceneName)
        {
            var stringBuilder = new StringBuilder();

            if (!char.IsLetter(sceneName[0]))
                stringBuilder.Append('_');

            foreach (var symbol in sceneName)
            {
                stringBuilder.Append(char.IsLetterOrDigit(symbol) ? symbol : '_');
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
