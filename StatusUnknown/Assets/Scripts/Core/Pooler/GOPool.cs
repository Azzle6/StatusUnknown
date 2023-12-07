namespace Core.Pooler
{
    using System.Collections.Generic;
    using UnityEngine;

    [System.Serializable]
    public class GOPool
    {
        public GameObject objPrefab;
        public uint baseCount;
        public Stack<GameObject> poolStack;
    }

    [System.Serializable]
    public class GOPoolKey
    {
        [HideInInspector] public string key;
        public GOPool pool;
    }
}