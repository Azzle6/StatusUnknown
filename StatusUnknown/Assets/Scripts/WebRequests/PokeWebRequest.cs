using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using StatusUnknown.CoreGameplayContent.Character;  

namespace StatusUnknown
{
    namespace WebRequest
    {
        /// <summary>
        /// This is a test class to generate a scriptable object from web API data. Will be moved/adapted to NotionWebRequest
        /// </summary>
        public class PokeWebRequest : WebRequestBase
        {
            [SerializeField] private List<Character> characters = new List<Character>();
            private readonly List<AbilityData> characterAbilitiesData = new List<AbilityData>();
            private StatsDataContainer[] allCharacterStatsContainers; 

            void Start()
            {
                for (int i = 1; i <= amountOfRequests; i++)
                {
                    StartCoroutine(WebRequestHandler.HandleRequest_GET(string.Concat(apiURL, i.ToString()), OnGetRequestComplete));
                }
            }

            protected override void Populate_OnGetComplete(UnityWebRequest uwb)
            {
                PopulateAbilities(uwb);
                PopulateStats(uwb);

                characters.Add(new Character(uwb, allCharacterStatsContainers));
            }

            protected override void Populate_OnPostComplete(UnityWebRequest uwb)
            {
                throw new NotImplementedException();
            }

            private void PopulateAbilities(UnityWebRequest uwb)
            {
                string[] jsonResponseToArray = JsonHelper.GetJsonObjects(uwb.downloadHandler.text, "ability");

                foreach (string jsonObj in jsonResponseToArray)
                {
                    characterAbilitiesData.Add(JsonUtility.FromJson<AbilityData>(jsonObj));
                }
            }

            private void PopulateStats(UnityWebRequest uwb)
            {
                string[] jsonResponseToArray = JsonHelper.GetJsonObjectArray(uwb.downloadHandler.text, "stats");
                allCharacterStatsContainers = new StatsDataContainer[jsonResponseToArray.Length];

                for (int i = 0; i < allCharacterStatsContainers.Length; i++)
                {
                    allCharacterStatsContainers[i] = JsonUtility.FromJson<StatsDataContainer>(jsonResponseToArray[i]);
                }
            }
        }
    }

    // this maybe be added to it's own separate class
    namespace CoreGameplayContent.Character
    {
        [Serializable]
        internal class Character
        {
            public string name;
            public StatsDataContainer[] Stats;

            public Character(UnityWebRequest request, StatsDataContainer[] statsDataContainers)
            {
                string temp = JsonHelper.GetJsonObject(request.downloadHandler.text, "species");
                name = JsonHelper.GetJsonObject(temp, "name");

                Stats = new StatsDataContainer[statsDataContainers.Length];
                Array.Copy(statsDataContainers, Stats, statsDataContainers.Length);
            }
        }

        [Serializable]
        internal class AbilityData
        {
            public string name;
            public string url;
        }

        [Serializable]
        internal class StatsDataContainer
        {
            public string base_stat;
            public StatData stat;
        }

        [Serializable]
        internal class StatData
        {
            public string name;
            public string url;
        }
    }
}
