using System;
using Sirenix.OdinInspector;

namespace Interactable
{
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "MapEncyclopedia", menuName = "CustomAssets/Map")]
    public class MapEncyclopedia : ScriptableObject
    {
        public Dictionary<string, TeleporterMapData> tpMapData => GetDictionary();
        public List<TeleporterMapData> maps;
        
        [Button("UpdateDictionary")]
        public Dictionary<string, TeleporterMapData> GetDictionary()
        {
            Dictionary<string, TeleporterMapData> result = new Dictionary<string, TeleporterMapData>();
            foreach (TeleporterMapData mapData in maps)
            {
                result.Add(mapData.sceneName, mapData);
            }

            return result;
        }

        /*private void Awake()
        {
            GetDictionary();
        }

        private void OnValidate()
        {
            GetDictionary();
        }*/
    }
    
    
}
