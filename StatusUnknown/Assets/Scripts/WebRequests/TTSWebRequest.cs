using CoreGameplayContent.VoiceLines;
using System;
using System.Collections.Generic;
using UnityEditor.Hardware;
using UnityEngine;
using UnityEngine.Networking;

public class TTSWebRequest : WebRequestBase
{
    [SerializeField] private VoiceLines voiceLines; 
    private readonly string filename = "unity-test-file.wav";
    private Dictionary<string, string> headers = new Dictionary<string, string>(); 

    void Start()
    {
        headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-rapidapi-host", "large-text-to-speech.p.rapidapi.com" },
            { "x-rapidapi-key", $"{authKey}" }
        };

        string postData = JsonUtility.ToJson(voiceLines);

        for (int i = 1; i <= amountOfRequests; i++)
        {
            StartCoroutine(WebRequestHandler.HandleRequest_POST(apiURL, postData, headers, OnPostRequestComplete));
        }
    }

    protected override void Populate_OnPostComplete(UnityWebRequest uwb)
    {
        Debug.Log("post response text : " + uwb.downloadHandler.text);
    }

    protected override void Populate_OnGetComplete(UnityWebRequest uwb)
    {
        Debug.Log("get response text : " + uwb.downloadHandler.text); 
        // StartCoroutine(WebRequestHandler.HandleRequest_GET(apiURL)); 
    }
}

namespace CoreGameplayContent.VoiceLines
{
    [Serializable]
    public class VoiceLines
    {
        [TextArea(10, 20)] public string text = "Text to speech technology allows you to convert text of unlimited sizes to humanlike voice audio files!";

    }
}
