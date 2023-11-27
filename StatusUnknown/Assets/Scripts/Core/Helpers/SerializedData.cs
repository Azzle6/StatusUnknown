namespace Core.Helpers
{
    using System;
    using UnityEngine;

    [Serializable]
    public class SerializedData
    {
        public Type type;
        public string data;

        public static SerializedData Serialize(object obj)
        {
            var result = new SerializedData()
            {
                type = obj.GetType(),
                data = JsonUtility.ToJson(obj)
            };
            Debug.Log($"Serialize. data : {result.data}, type :{result.type}.");
            return result;
        }
        
        public static object Deserialize(SerializedData sd)
        {
            Debug.Log($"Deserialize.  data : {sd.data}, type :{sd.type}.");
            return JsonUtility.FromJson(sd.data, sd.type);
        }
    }
}
