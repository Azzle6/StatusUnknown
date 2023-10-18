using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using StatusUnknown.CoreGameplayContent; 

namespace StatusUnknown
{
    namespace WebRequest
    {
        public class PokeWebRequest : WebRequestBase
        {
            [SerializeField] private List<Character> characters = new List<Character>();
            private readonly List<AbilityData> characterAbilitiesData = new List<AbilityData>();
            private StatsDataContainer[] allCharacterStatsContainers; 

            void Start()
            {
                for (int i = 1; i <= amountOfRequests; i++)
                {
                    StartCoroutine(WebRequestHandler.HandleRequest(string.Concat(apiURL, i.ToString()), OnRequestComplete));
                }
            }

            protected override void Populate(UnityWebRequest uwb)
            {
                PopulateAbilities(uwb);
                PopulateStats(uwb);

                characters.Add(new Character(uwb, allCharacterStatsContainers));
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

    namespace CoreGameplayContent
    {
        [Serializable]
        public class Character
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
        public class AbilityData
        {
            public string name;
            public string url;
        }

        [Serializable]
        public class StatsDataContainer
        {
            public string base_stat;
            public StatData stat;
        }

        [Serializable]
        public class StatData
        {
            public string name;
            public string url;
        }
    }
}
