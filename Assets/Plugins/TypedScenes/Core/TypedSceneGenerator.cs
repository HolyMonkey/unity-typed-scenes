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
        private const string Namespace = "IJunior.TypedScene";
        private const string SavingDirectory = "Assets/Scenes/Typed/";
        private const string ClassExtension = ".cs";

        public static void Generate(string path)
        {
            var className = GetValidClassName(Path.GetFileNameWithoutExtension(path));

            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(Namespace);
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

            Directory.CreateDirectory(SavingDirectory);

            using (StreamWriter sourceWriter = new StreamWriter(SavingDirectory + name + ClassExtension))
            {
                provider.GenerateCodeFromCompileUnit(targetUnit, sourceWriter, options);
            }

            AssetDatabase.ImportAsset(SavingDirectory + name + ClassExtension, ImportAssetOptions.ForceUpdate);
        }
    }
}
