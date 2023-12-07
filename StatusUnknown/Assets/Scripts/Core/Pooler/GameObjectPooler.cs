namespace Core.Pooler
{
    using System.Collections.Generic;
    using UnityEngine;
    using Core;
    public class GameObjectPooler : MonoSingleton<GameObjectPooler>
    {
        private Dictionary<string, GOPool> pools = new Dictionary<string, GOPool>();
        [SerializeField] private List<GOPoolKey> poolToInit;
        private GameObject tempPooledObj;
        private string tempString;

        private void Start()
        {
            InitPools();
        }
        public void AddPool(GOPoolKey addedPool)
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

            foreach (GOPoolKey key in poolToInit)
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
        private void AddInstance(GOPool pool)
        {
            tempPooledObj = Instantiate(pool.objPrefab, transform);
            tempPooledObj.gameObject.SetActive(false);
            pool.poolStack.Push(tempPooledObj);
        }
        //Use this to get an object from the pool
        public T GetPooledObject<T>(string key)
        {
            if (!pools.ContainsKey(key))
            {
                Debug.LogError("Pooler: " + key + " doesn't exist");
                return default;
            }
            if (pools[key].poolStack.Count == 0)
            {
                AddInstance(pools[key]);
            }
            tempPooledObj = pools[key].poolStack.Pop();
            tempPooledObj.gameObject.SetActive(true);
            tempPooledObj.transform.parent = null;
            return tempPooledObj.GetComponent<T>();
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
