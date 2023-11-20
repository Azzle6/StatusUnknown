namespace Core.SingletonsSO
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;

    public class SingletonSO<T> : ScriptableObject where T : SingletonSO<T>
    {
        private static T instance = null;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    var op = Addressables.LoadAssetAsync<T>("SingletonSO");

                    T result = op.WaitForCompletion();

                    if (result == null)
                        Debug.LogWarning($"Cannot find instance of {typeof(T)}.");
                    
                    instance = result;
                }
                return instance;
            }
        }
    }
}
