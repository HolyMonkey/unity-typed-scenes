using System;

namespace IJunior.TypedScene
{
	interface ISceneLoadHandler<T>
    {
        void OnSceneLoaded(T argument);
    }
}