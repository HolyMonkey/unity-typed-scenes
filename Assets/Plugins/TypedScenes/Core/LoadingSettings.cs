using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace IJunior.TypedScenes
{
    public class LoadingSettings
    {
        private Action _applyModelAction;
        
        private LoadingSettings() {}

        public void Apply()
        {
            try
            {
                _applyModelAction?.Invoke();
            }
            catch (Exception e)
            {
                Debug.LogError($"Can not apply loading model. Exception: {e}");
            }
        }

        public void Register<T>(T loadingModel)
        {
            _applyModelAction = () =>
            {
                foreach (var rootObjects in SceneManager.GetActiveScene().GetRootGameObjects())
                {
                    foreach (var handler in rootObjects.GetComponentsInChildren<ISceneLoadHandler<T>>())
                    {
                        handler.OnSceneLoaded(loadingModel);
                    }
                }
            };
        }

        public void Clear()
        {
            _applyModelAction = null;
        }

        public static LoadingSettings Create()
        {
            return new LoadingSettings();
        }
    }
}