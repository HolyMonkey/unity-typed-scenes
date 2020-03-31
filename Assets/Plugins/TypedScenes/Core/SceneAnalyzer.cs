using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class SceneAnalyzer
    {
        public static IEnumerable<Type> GetLoadingParameters(string sceneGUID, bool includeNullParameter = true)
        {
            var currentScene = SceneManager.GetActiveScene().path;
            var path = AssetDatabase.GUIDToAssetPath(sceneGUID);
            var rootObjects = EditorSceneManager.OpenScene(path).GetRootGameObjects();
            var loadParameters = new HashSet<Type>();

            if (includeNullParameter)
                loadParameters.Add(null);

            foreach (var gameObject in rootObjects)
            {
                var components = gameObject.GetComponentsInChildren<Component>();
                foreach (var component in components)
                {
                    var type = component.GetType();
                    if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISceneLoadHandler<>)))
                    {
                        var loadMethods = type.GetMethods().Where(method => method.Name == "OnSceneLoaded");
                        foreach(var method in loadMethods)
                        {
                            loadParameters.Add(method.GetParameters()[0].ParameterType);
                        }
                    }
                }
            }

            EditorSceneManager.OpenScene(currentScene);
            return loadParameters;
        }
    } 
}
