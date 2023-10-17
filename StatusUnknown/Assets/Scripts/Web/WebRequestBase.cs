using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestBase : MonoBehaviour
{
    [SerializeField] private string apiURL;
    [SerializeField, Range(0, 10)] private int amountOfCharacters = 2; 
    [SerializeField] private List<Character> characters = new List<Character>();    

    void Start()
    {
        for (int i = 1; i <= amountOfCharacters; i++) 
        {
            StartCoroutine(GetPokemon(i.ToString()));
        }
    }

    private IEnumerator GetPokemon(string index)
    {
        using UnityWebRequest request = UnityWebRequest.Get(apiURL + $"{index}/");
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            // ABILITIES
            string[] allCharacterAbilitiesJson = JsonHelper.GetJsonObjects(request.downloadHandler.text, "ability");
            List<AbilityData> characterAbilitiesData = new List<AbilityData>();

            foreach (string jsonObj in allCharacterAbilitiesJson)
            {
                characterAbilitiesData.Add(JsonUtility.FromJson<AbilityData>(jsonObj));
            }
            Debug.Log(characterAbilitiesData);


            // STATS
            string[] allCharacterStatsJson = JsonHelper.GetJsonObjectArray(request.downloadHandler.text, "stats");
            StatsDataContainer[] allCharacterStatsContainers = new StatsDataContainer[allCharacterStatsJson.Length];

            for (int i = 0; i < allCharacterStatsContainers.Length; i++)
            {
                allCharacterStatsContainers[i] = JsonUtility.FromJson<StatsDataContainer>(allCharacterStatsJson[i]);
            }

            characters.Add(new Character(request, allCharacterStatsContainers));
        }
        else
        {
            Debug.LogError("Web Request did not succeed");
        }
    }
}

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

