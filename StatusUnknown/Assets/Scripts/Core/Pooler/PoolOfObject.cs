namespace Core.Pooler
{
    using System.Collections.Generic;
    using SingletonsSO;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PoolOfObject", menuName = "CustomAsset/PoolOfObject", order = 1)]
    public class PoolOfObject : SingletonSO<PoolOfObject>
    {
        public List<PoolKey> pool;
    
        public void AddPool(PoolKey addedPool)
        {
            pool.Add(addedPool);
        }
    }
}
