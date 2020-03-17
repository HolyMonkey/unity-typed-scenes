using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace IJunior.TypedScene
{
    public class TypedSceneManager : MonoBehaviour
    {
        public static void Generate(string path)
        {
            var className = GetValidClassName(Path.GetFileNameWithoutExtension(path));

            if (AlreadyExists(className))
            {
                Debug.LogError("A scene with the same name already exists. Scene and Typed Scene renamed");

                var directory = Path.GetDirectoryName(path);
                var newSceneName = className + GetAccessiblePostfix(className);
                File.Move(path, directory + "/" + newSceneName + TypedSceneSettings.SceneExtension);
                path = directory + "/" + newSceneName + TypedSceneSettings.SceneExtension;
            }

            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(TypedSceneSettings.Namespace);
            var targetClass = new CodeTypeDeclaration(className);
            targetClass.BaseTypes.Add("TypedScene");

            var pathConstant = new CodeMemberField(typeof(string), "Path");
            pathConstant.Attributes = MemberAttributes.Private | MemberAttributes.Const;
            pathConstant.InitExpression = new CodePrimitiveExpression(path);
            targetClass.Members.Add(pathConstant);

            var loadMethod = new CodeMemberMethod();
            loadMethod.Name = "Load";
            loadMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            loadMethod.Statements.Add(new CodeSnippetExpression("LoadScene(Path)"));
            targetClass.Members.Add(loadMethod);

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            Save(className, targetUnit);
        }

        public static void Delete(string sceneName)
        {
            var className = GetValidClassName(sceneName);
            var path = TypedSceneSettings.SavingDirectory + className + TypedSceneSettings.ClassExtension;

            if (File.Exists(path))
            {
                AssetDatabase.DeleteAsset(path);
                AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
            }
        }

        private static string GetValidClassName(string name)
        {
            var stringBuilder = new StringBuilder();

            if (!char.IsLetter(name[0]))
                stringBuilder.Append('_');

            foreach (var symbol in name)
            {
                stringBuilder.Append(char.IsLetterOrDigit(symbol) ? symbol : '_');
            }

            return stringBuilder.ToString();
        }

        private static void Save(string name, CodeCompileUnit targetUnit)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            Directory.CreateDirectory(TypedSceneSettings.SavingDirectory);

            using (StreamWriter sourceWriter = new StreamWriter(TypedSceneSettings.SavingDirectory + name + TypedSceneSettings.ClassExtension))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }

            AssetDatabase.ImportAsset(TypedSceneSettings.SavingDirectory + name + TypedSceneSettings.ClassExtension, ImportAssetOptions.ForceUpdate);
        }

        private static bool AlreadyExists(string className)
        {
            var path = TypedSceneSettings.SavingDirectory + className + TypedSceneSettings.ClassExtension;
            return File.Exists(path);
        }

        private static string GetAccessiblePostfix(string className)
        {
            var number = 0;

            while (AlreadyExists(className + number))
                number++;

            return number.ToString();
        }
    } 
}
