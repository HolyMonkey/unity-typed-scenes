using System;

namespace IJunior.TypedScenes
{
    interface ISceneLoadHandler<T>
    {
        void OnSceneLoaded(T argument);
    }
}
