namespace Core.Helpers
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<TKey> keys = new List<TKey>();

        [SerializeField] private List<TValue> values = new List<TValue>();

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            this.keys.Clear();
            this.values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                this.keys.Add(pair.Key);
                this.values.Add(pair.Value);
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (this.keys.Count != this.values.Count)
                throw new Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

            for (int i = 0; i < this.keys.Count; i++)
                this.Add(this.keys[i], this.values[i]);
        }
    }
}