using System;
using UnityEngine;

namespace Core.SingletonsSO
{
    public class SingletonSO<T> : ScriptableObject where T : SingletonSO<T>
    {
        private static T instance = null;

        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    T[] results = Resources.FindObjectsOfTypeAll<T>();
                    if (results.Length == 0)
                    {
                        throw new Exception($"Cannot find instance of {typeof(T)}.");
                    }
                    if (results.Length > 1)
                    {
                        throw new Exception($"Multiple instances of {typeof(T)} exists. Only one will be used, delete others.");
                    }

                    instance = results[0];
                    instance.hideFlags = HideFlags.DontUnloadUnusedAsset;
                }

                return instance;
            }
        }
    }
}
