using UnityEngine.Pool;

namespace Core.Pooler
{
    using System.Collections.Generic;
    using UnityEngine;
    using Core;

    public class ComponentPooler : MonoSingleton<ComponentPooler>
    {
        private Dictionary<string, IObjectPool<Component>> pools = new Dictionary<string, IObjectPool<Component>>();
        private Dictionary<GameObject, Component> objectToComponent = new Dictionary<GameObject, Component>();
        
        
        public void CreatePool<T>(T prefab, int baseCount) where T : Component
        {
            string key = prefab.gameObject.name;
            if (pools.ContainsKey(key))
            {
                return;
            }

            IObjectPool<Component> newPool = new ObjectPool<Component>(() => (Object.Instantiate(prefab)), ActionOnGet, ActionOnRelease,null, false,baseCount);
            AddPool<Component>(prefab.gameObject.name, newPool);
            
        }

        public void AddPool<T>(string key, IObjectPool<Component> pool) where T : Object
        {
            if (pools.ContainsKey(key))
            {
                return;
            }
            pools.Add(key, pool);
        }

        public T GetPooledObject<T>(string key) where T : Component
        {
            if (!pools.ContainsKey(key))
            {
                return default;
            }
            T component = (T)pools[key].Get();
            objectToComponent[component.gameObject] = component;
            return component;
        }

        public void ReturnObjectToPool(GameObject returnedObj)
        {
            string key = returnedObj.name;
            key = key.Substring(0, key.Length - 7);

            returnedObj.gameObject.SetActive(false);
            returnedObj.transform.parent = transform;

            if (pools.ContainsKey(key))
            {
                if (objectToComponent.TryGetValue(returnedObj, out Component component))
                {
                    pools[key].Release(component);
                    objectToComponent.Remove(returnedObj);
                }
            }
        }

        public void ActionOnGet<T>(T obj) where T : Component
        {
            obj.gameObject.SetActive(true);
        }

        public void ActionOnRelease<T>(T obj) where T : Component
        {
            obj.gameObject.SetActive(false);
        }


    }
    
}