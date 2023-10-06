namespace Core.Pooler
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class Pool
    {
        public GameObject objPrefab;
        public uint baseCount;
        public Stack<GameObject> poolStack;
    }

    [System.Serializable]
    public class PoolKey
    {
        [HideInInspector] public string key;
        public Pool pool;
    }
}