using CoreGameplayContent.VoiceLines;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

// api URL https://large-text-to-speech.p.rapidapi.com/tts
// auth key 1cc4d2218bmshe456bc8d87002fap159ec0jsnd918b3538dc3 (NOT DEPLOYED)

public class TTSWebRequest : WebRequestBase
{
    [SerializeField] private VoiceLines voiceLines; 
    private readonly string filename = "unity-test-file.wav";
    private Dictionary<string, string> headers = new Dictionary<string, string>(); 

    private string requestID;
    private string requestURL;
    private Data data = new Data();  

    void Start()
    {
        // PostRequest(); 
        GetRequest();   
    }

    private void PostRequest()
    {
        headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-rapidapi-host", "large-text-to-speech.p.rapidapi.com" },
            { "x-rapidapi-key", $"{authKey}" }
        };

        string postData = JsonUtility.ToJson(voiceLines);
        StartCoroutine(WebRequestHandler.HandleRequest_POST(apiURL, postData, headers, OnPostRequestComplete));
    }

    protected override void Populate_OnPostComplete(UnityWebRequest uwb)
    {
        // use DownloadHandler ?
        /* _infos.id = JsonHelper.GetJsonObject(uwb.downloadHandler.text, "id");
        requestID = infos.id; */

        if (debugPostMessage)
        {
            Debug.Log("post response id : " + requestID);
        }

        GetRequest(); 
    }

    private void GetRequest()
    {
        // DEBUG
        data.@params = new()
        {
            id = "79f47a19-809d-4031-af1c-2750e8c85144" 
        };
        // "x-amzn-requestid":"79f47a19-809d-4031-af1c-2750e8c85144"

        headers = new Dictionary<string, string>
        {           
            { "x-rapidapi-host", "large-text-to-speech.p.rapidapi.com" },
            { "x-rapidapi-key", $"{authKey}" }
        };

        // KeyValuePair<string, string> data = new KeyValuePair<string, string>("id", "79f47a19-809d-4031-af1c-2750e8c85144"); 
        // string getData = JsonUtility.ToJson(data);

        string getData = JsonUtility.ToJson(data);
        StartCoroutine(WebRequestHandler.HandleRequest_GET(apiURL, headers, getData, OnGetRequestComplete));
    }


    protected override void Populate_OnGetComplete(UnityWebRequest uwb)
    {
        if (debugGetMessage) 
        {
            Debug.Log($"Post response : {uwb.downloadHandler.text}");
        }

        if (requestURL == string.Empty)
        {
            requestURL = JsonHelper.GetJsonObject(uwb.downloadHandler.text, "url");
            Debug.Log("get response url : " + requestURL); // TODO custom path in unity folder Assets/Audio

            StartCoroutine(WebRequestHandler.HandleRequest_GET(requestURL, OnGetRequestComplete));
        } 
        else
        {
            string path = "Assets/Audio/Files"; 
            Debug.Log($"Successfully retrieved data from url : {requestURL}. \n " +
                      $"Stored at {path}");
        }
    }
}

namespace CoreGameplayContent.VoiceLines
{
    [Serializable]
    public class VoiceLines
    {
        [TextArea(10, 20)] public string text = "Text to speech technology allows you to convert text of unlimited sizes to humanlike voice audio files!";

    }

    [Serializable]
    public class Data
    {
        public Infos @params;
    }

    [Serializable]
    public class Infos
    {
        public string id; 
    }
}
