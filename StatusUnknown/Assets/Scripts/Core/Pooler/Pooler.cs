using Core.SingletonsSO;

namespace Core.Pooler
{
    using System.Collections.Generic;
    using UnityEngine;
    using Core;
    public class Pooler : MonoSingleton<Pooler>
    {
        private Dictionary<string, Pool> pools = new Dictionary<string, Pool>();
        [SerializeField] private List<PoolKey> poolToInit;
        private GameObject tempPooledObj;
        private string tempString;

        private void Start()
        {
            InitPools();
        }
        public void AddPool(PoolKey addedPool)
        {
            poolToInit.Add(addedPool);
        }

        public void ClearPool()
        {
            poolToInit.Clear();
        }

        //Add all pools to dictionary
        [ContextMenu("Init Pools")]
        public void InitPools()
        { 
            if (poolToInit == null) return;

            foreach (PoolKey key in poolToInit)
            {
                key.key = key.pool.objPrefab.name;
                pools.Add(key.key, key.pool);
            }

            PopulatePools();
        }
        //Instantiate all objects in pools and spawn stack
        public void PopulatePools()
        {
            if (poolToInit == null) 
                return;
                
            for (int x = 0; x < poolToInit.Count; x++)
            {
                poolToInit[x].pool.poolStack = new Stack<GameObject>();
                for (int y = 0; y < poolToInit[x].pool.baseCount; y++) 
                    AddInstance(poolToInit[x].pool);
            }
            
        }
        private void AddInstance(Pool pool)
        {
            tempPooledObj = Instantiate(pool.objPrefab, transform);
            tempPooledObj.gameObject.SetActive(false);
            pool.poolStack.Push(tempPooledObj);
        }
        //Use this to get an object from the pool
        public GameObject GetPooledObject(string key)
        {
            if (pools.ContainsKey(key))
            {
                if (pools[key].poolStack.Count == 0)
                    AddInstance(pools[key]);
                tempPooledObj = pools[key].poolStack.Pop();
                tempPooledObj.gameObject.SetActive(true);
                tempPooledObj.transform.parent = null;
                return tempPooledObj;
            }
            else
            {
                Debug.LogError("Pooler does not contain key: " + key);
                return null;
            }
        }
        //Use this to return an object to the pool
        public void ReturnObjectToPool(GameObject returnedObj)
        {
            tempString = returnedObj.name;
            tempString = tempString.Substring(0, tempString.Length - 7);
            returnedObj.gameObject.SetActive(false);
            returnedObj.transform.parent = transform;
            pools[tempString].poolStack.Push(returnedObj);
        }
    }
}
