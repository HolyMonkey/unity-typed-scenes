using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using UnityEngine;

namespace IJunior.TypedScene
{
    public class TypedSceneGenerator : MonoBehaviour
    {
        private const string _namespace = "IJunior.TypedScene";
        private const string _savingDirectory = "Assets/Scenes/TypedScenes/";
        private const string _classExtension = ".cs";

        public static void Generate(string path)
        {
            var className = Path.GetFileNameWithoutExtension(path);
            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(_namespace);
            var targetClass = new CodeTypeDeclaration(className);
            targetClass.BaseTypes.Add("TypedScene");

            var pathConstant = new CodeMemberField(typeof(string), "_path");
            pathConstant.Attributes = MemberAttributes.New | MemberAttributes.Family | MemberAttributes.Const;
            pathConstant.InitExpression = new CodePrimitiveExpression(path);
            targetClass.Members.Add(pathConstant);

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);
            Save(className, targetUnit);
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
        }
    } 
}
