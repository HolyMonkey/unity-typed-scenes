using System;
using System.Collections.Generic;
using System.IO;
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
            var loadParameters = new HashSet<Type>();

            if (includeNullParameter)
                loadParameters.Add(null);

            TryAnalyseScene(sceneGUID, scene =>
            {
                var componentTypes = GetAllTypes(scene);

                foreach(var type in componentTypes)
                {
                    if (type.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(ISceneLoadHandler<>)))
                    {
                        var loadMethods = type.GetMethods().Where(method => method.Name == "OnSceneLoaded");
                        foreach (var method in loadMethods)
                        {
                            loadParameters.Add(method.GetParameters()[0].ParameterType);
                        }
                    }
                }
            });

            return loadParameters;
        }

        public static bool TryAddTypedProcessor(string sceneGUID)
        {
            var added = false;

            TryAnalyseScene(sceneGUID, scene =>
            {
                var componentTypes = GetAllTypes(scene);

                if (!componentTypes.Contains(typeof(TypedProcessor)))
                {
                    var gameObject = new GameObject("TypedProcessor");
                    gameObject.AddComponent<TypedProcessor>();
                    scene.GetRootGameObjects().Append(gameObject);
                    added = true;
                }
            });

            return added;
        }

        private static void TryAnalyseScene(string sceneGUID, Action<Scene> analyser)
        {
            var scene = SceneManager.GetActiveScene();
            var currentPath = scene.path;
            var targetPath = AssetDatabase.GUIDToAssetPath(sceneGUID);

            if (targetPath == currentPath)
            {
                analyser(scene);
                return;
            }

            if (File.Exists(targetPath))
            {
                scene = EditorSceneManager.OpenScene(targetPath, OpenSceneMode.Additive);
                analyser(scene);
                EditorSceneManager.CloseScene(scene, true);
            }
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
