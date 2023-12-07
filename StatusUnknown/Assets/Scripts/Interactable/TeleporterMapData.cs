namespace Interactable
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    [System.Serializable]
    public class TeleporterMapData 
    {
        public string sceneName;
        public VisualTreeAsset sceneMap;
        public List<TeleporterData> teleporterData;
    }

    [System.Serializable]
    public class TeleporterData
    {
        public Vector3 teleporterPos;
    }



}

