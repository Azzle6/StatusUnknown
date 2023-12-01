using System;

namespace Interactable
{
    using System.Collections.Generic;
    using UnityEngine;
    
    [CreateAssetMenu(fileName = "MapEncyclopedia", menuName = "CustomAssets/Map")]
    public class MapEncyclopedia : ScriptableObject
    {
        public Dictionary<string, TeleporterMapData> tpMapData = new Dictionary<string, TeleporterMapData>();
        public List<TeleporterMapData> maps;
        
        public void UpdateDictionary()
        {
            tpMapData.Clear();
            foreach (TeleporterMapData mapData in maps)
            {
                tpMapData.Add(mapData.sceneName, mapData);
            }
        }

        private void OnValidate()
        {
            UpdateDictionary();
        }
    }
    
    
}
