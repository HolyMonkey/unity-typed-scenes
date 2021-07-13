#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public static class SceneAnalyzer
    {
        public static IEnumerable<Type> GetLoadingParameters(AnalyzableScene analyzableScene)
        {
            var scene = analyzableScene.Scene;
            var loadParameters = new List<Type> {null};
            var componentTypes = GetAllTypes(scene);

            loadParameters.AddRange(componentTypes
                .Where(type => type.GetInterfaces()
                    .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISceneLoadHandler<>)))
                .SelectMany(type => type.GetMethods().Where(method => method.Name == "OnSceneLoaded"),
                    (type, method) => method.GetParameters()[0].ParameterType));

            if (loadParameters.Count > 1)
                loadParameters.Remove(null);

            return loadParameters;
        }

        public static bool TryAddTypedProcessor(AnalyzableScene analyzableScene)
        {
            var scene = analyzableScene.Scene;
            var componentTypes = GetAllTypes(scene);
            
            if (componentTypes.Contains(typeof(TypedProcessor))) return false;

            var gameObject = new GameObject("TypedProcessor");
            gameObject.AddComponent<TypedProcessor>();
            scene.GetRootGameObjects().Append(gameObject);
            Undo.RegisterCreatedObjectUndo(gameObject, "Typed processor added");
            EditorSceneManager.SaveScene(scene);
            return true;
        }

        private static IEnumerable<Component> GetAllComponents(Scene activeScene)
        {
            var rootObjects = activeScene.GetRootGameObjects();
            var components = new List<Component>();

            foreach (var gameObject in rootObjects)
            {
                components.AddRange(gameObject.GetComponentsInChildren<Component>());
            }

            return components;
        }

        private static IEnumerable<Type> GetAllTypes(Scene activeScene)
        {
            var components = GetAllComponents(activeScene);
            var types = new HashSet<Type>();

            foreach (var component in components)
            {
                types.Add(component.GetType());
            }

            return types;
        }
    }
}
#endif
