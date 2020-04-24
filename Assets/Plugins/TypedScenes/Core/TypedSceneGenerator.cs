using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class TypedSceneGenerator
    {
        public static string Generate(string className, string GUID)
        {
            var targetUnit = new CodeCompileUnit();
            var targetNamespace = new CodeNamespace(TypedSceneSettings.Namespace);
            var targetClass = new CodeTypeDeclaration(className);
            targetNamespace.Imports.Add(new CodeNamespaceImport("UnityEngine.SceneManagement"));
            targetClass.BaseTypes.Add("TypedScene");

            AddConstantValue(targetClass, typeof(string), "GUID", GUID);

            var loadingParameters = SceneAnalyzer.GetLoadingParameters(GUID);
            foreach (var loadingParameter in loadingParameters)
            {
                AddLoadingMethod(targetClass, loadingParameter);
            }

            targetNamespace.Types.Add(targetClass);
            targetUnit.Namespaces.Add(targetNamespace);

            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions();
            options.BracingStyle = "C";

            var code = new StringWriter();
            provider.GenerateCodeFromCompileUnit(targetUnit, code, options);

            return code.ToString();
        }

        private static void AddConstantValue(CodeTypeDeclaration targetClass, Type type, string name, string value)
        {
            var pathConstant = new CodeMemberField(type, name);
            pathConstant.Attributes = MemberAttributes.Private | MemberAttributes.Const;
            pathConstant.InitExpression = new CodePrimitiveExpression(value);
            targetClass.Members.Add(pathConstant);
        }

        private static void AddLoadingMethod(CodeTypeDeclaration targetClass, Type parameterType = null)
        {
            var loadMethod = new CodeMemberMethod();
            loadMethod.Name = "Load";
            loadMethod.Attributes = MemberAttributes.Public | MemberAttributes.Static;

            var loadingStatement = "LoadScene(GUID, loadSceneMode)";

            if (parameterType != null)
            {
                var parameter = new CodeParameterDeclarationExpression(parameterType, "argument");
                loadMethod.Parameters.Add(parameter);
                loadingStatement = "LoadScene(GUID, loadSceneMode, argument)";
            }

            var loadingModeParameter = new CodeParameterDeclarationExpression(typeof(LoadSceneMode).Name, "loadSceneMode = LoadSceneMode.Single");
            loadMethod.Parameters.Add(loadingModeParameter);

            loadMethod.Statements.Add(new CodeSnippetExpression(loadingStatement));
            targetClass.Members.Add(loadMethod);
        }
    }
}
