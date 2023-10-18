using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using StatusUnknown.CoreGameplayContent; 

namespace StatusUnknown
{
    namespace WebRequest
    {
        public class PokeWebRequest : MonoBehaviour
        {
            [SerializeField] private string apiURL;
            [SerializeField, Range(0, 10)] private int amountOfCharacters = 3;
            [SerializeField] private List<Character> characters = new List<Character>();
            private Action<UnityWebRequest> OnComplete;

            private void OnEnable()
            {
                OnComplete += PopulateCharacters;
            }

            void Start()
            {
                for (int i = 1; i <= amountOfCharacters; i++)
                {
                    StartCoroutine(WebRequestHandler.HandleRequest(string.Concat(apiURL, i.ToString()), OnComplete));
                }
            }

            private void PopulateCharacters(UnityWebRequest uwb)
            {
                // ABILITIES
                string[] allCharacterAbilitiesJson = JsonHelper.GetJsonObjects(uwb.downloadHandler.text, "ability");
                List<AbilityData> characterAbilitiesData = new List<AbilityData>();

                foreach (string jsonObj in allCharacterAbilitiesJson)
                {
                    characterAbilitiesData.Add(JsonUtility.FromJson<AbilityData>(jsonObj));
                }
                Debug.Log(characterAbilitiesData);


                // STATS
                string[] allCharacterStatsJson = JsonHelper.GetJsonObjectArray(uwb.downloadHandler.text, "stats");
                StatsDataContainer[] allCharacterStatsContainers = new StatsDataContainer[allCharacterStatsJson.Length];

                for (int i = 0; i < allCharacterStatsContainers.Length; i++)
                {
                    allCharacterStatsContainers[i] = JsonUtility.FromJson<StatsDataContainer>(allCharacterStatsJson[i]);
                }

                characters.Add(new Character(uwb, allCharacterStatsContainers));
            }


            private void OnDisable()
            {
                OnComplete -= PopulateCharacters;
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
