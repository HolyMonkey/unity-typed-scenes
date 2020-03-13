using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace IJunior.TypedScene
{
    public class TypedSceneGenerator : MonoBehaviour
    {
        private const string _namespace = "IJunior.TypedScene";
        private const string _savingDirectory = "Assets/Plugins/TypedScenes/Samples/";
        private const string _classExtension = ".cs";

        public static void Generate(string path)
        {
            var className = GetValidClassName(Path.GetFileNameWithoutExtension(path));

            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(_namespace);
            var targetClass = new CodeTypeDeclaration(className);
            targetClass.BaseTypes.Add("TypedScene");

            var pathConstant = new CodeMemberField(typeof(string), "_path");
            pathConstant.Attributes = MemberAttributes.Private | MemberAttributes.Const;
            pathConstant.InitExpression = new CodePrimitiveExpression(path);
            targetClass.Members.Add(pathConstant);

            var loadMethod = new CodeMemberMethod();
            loadMethod.Name = "Load";
            loadMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;
            loadMethod.Statements.Add(new CodeSnippetExpression("LoadScene(_path)"));
            targetClass.Members.Add(loadMethod);

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            Save(className, targetUnit);
        }

        private static string GetValidClassName(string name)
        {
            var stringBuilder = new StringBuilder();

            if (!char.IsLetter(name[0]))
            {
                stringBuilder.Append('_');
            }

            foreach(var symbol in name)
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

            Directory.CreateDirectory(_savingDirectory);

            using (StreamWriter sourceWriter = new StreamWriter(_savingDirectory + name + _classExtension))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }

            AssetDatabase.ImportAsset(_savingDirectory + name + _classExtension, ImportAssetOptions.ForceUpdate);
        }
    } 
}
