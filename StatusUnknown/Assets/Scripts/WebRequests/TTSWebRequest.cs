using CoreGameplayContent.VoiceLines;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

// api URL https://large-text-to-speech.p.rapidapi.com/tts
// uri GET ?id=...
// auth key "..." (NOT DEPLOYED)

public enum AudioFileExtension { wav, mp3, ogg }

public class TTSWebRequest : WebRequestBase
{
    [SerializeField] private VoiceLines voiceLinesToPost;
    [SerializeField] private string fileName = "my-file-name";
    [SerializeField] private AudioFileExtension fileExtension = AudioFileExtension.wav;

    private string fileFullName; 
    private Dictionary<string, string> headers = new Dictionary<string, string>(); 
    private string getURI;
    private const string PATH_TO_AUDIO_FILES = "/Audio/Files/";


    [Space, Header("-- DEBUG --")]  
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private GETResponse responseSO;
    private POSTResponseObj postResponseObj = new POSTResponseObj();
    private GETResponseObj getResponseObj  = new GETResponseObj();  

    private void Start()
    {
        if (useScriptableIfProvided && !string.IsNullOrEmpty(responseSO.ID))
        {
            if (!responseSO) return; 

            Debug.Log("skipping POST request"); 

            postResponseObj.id = responseSO.ID;
            postResponseObj.eta = "15"; // :D
            postResponseObj.text = responseSO.GetText();  

            GetRequest();
            return; 
        }

        PostRequest();
    }

    [ContextMenu("Generate TTS file")]
    private void PostRequest()
    {
        Debug.Log("doing POST"); 
        fileFullName = string.Concat(fileName, ".", fileExtension);

        headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-rapidapi-host", "large-text-to-speech.p.rapidapi.com" },
            { "x-rapidapi-key", $"{authKey}" }
        };

        responseSO.SetText(voiceLinesToPost.text); 

        string postData = JsonUtility.ToJson(voiceLinesToPost);
        StartCoroutine(WebRequestHandler.HandleRequest_POST(apiURL, postData, headers, OnPostRequestComplete));
    }

    protected override void Populate_OnPostComplete(UnityWebRequest uwb)
    {
        postResponseObj = JsonUtility.FromJson<POSTResponseObj>(uwb.downloadHandler.text);
        postResponseObj.id = string.Concat("?id=", postResponseObj.id);

        responseSO.ID = postResponseObj.id;

        if (debugPostMessage)
        {
            Debug.Log("post response id : " + postResponseObj.id);
        }

        GetRequest(); 
    }

    private void GetRequest()
    {
        Debug.Log("doing GET");

        headers = new Dictionary<string, string>
        {           
            { "x-rapidapi-host", "large-text-to-speech.p.rapidapi.com" },
            { "x-rapidapi-key", $"{authKey}" }
        };

        getURI = string.Concat(apiURL, postResponseObj.id);
        responseSO.Uri = getURI; 

        StartCoroutine(WebRequestHandler.HandleRequest_GET(getURI, headers, OnGetRequestComplete));
    }

    protected override void Populate_OnGetComplete(UnityWebRequest uwb)
    {
        if (string.IsNullOrEmpty(getResponseObj.url))
        {
            if (debugGetMessage)
            {
                Debug.Log($"GET response : {uwb.downloadHandler.text}");
            }

            if (!useScriptableIfProvided)
            {
                getResponseObj = JsonUtility.FromJson<GETResponseObj>(uwb.downloadHandler.text);

                if (responseSO && string.IsNullOrEmpty(responseSO.Url)) { responseSO.Url = getResponseObj.url;  }
            }
            else
            {
                Debug.Log("getting data from scriptable object"); 
                getResponseObj.url = responseSO.Url; 
            }

            StartCoroutine(WebRequestHandler.HandleRequest_GET_MEDIA(getResponseObj.url, OnGetRequestComplete));
        }
        else
        {
            Debug.Log("playing audio file downloaded from server"); 
            DownloadHandlerAudioClip dlHandler = uwb.downloadHandler as DownloadHandlerAudioClip;

            AudioClip _clip = dlHandler?.audioClip;
            _clip.name = fileName; 
            audioSource.PlayOneShot(_clip);

            // TODO : SAVE AUDIO FILE TO UNTIY
            // dlHandler.data; 
            // Debug.Log("saving audio file to Unity");


            // string fullPath = string.Concat(Application.dataPath, PATH_TO_AUDIO_FILES, fileFullName);
            /* string fullPath = string.Concat("C:/Users/f.nossin/Desktop/Audio/", fileFullName);
            var myFile = File.Create(fullPath, uwb.downloadHandler.data.Length);
            File.WriteAllBytes(fullPath, uwb.downloadHandler.data);
            myFile.Close();

            string fullPathOther = string.Concat("C:/Users/f.nossin/Desktop/Audio/", "other.mp3");
            var myOtherFile = File.Create(fullPathOther, uwb.downloadHandler.data.Length);
            File.WriteAllBytes(fullPathOther, uwb.downloadHandler.data);
            myOtherFile.Close();

            string fullPathAgain = string.Concat("C:/Users/f.nossin/Desktop/Audio/", "again.mp3");
            var myAgainFile = File.Open(fullPathAgain, FileMode.Open, FileAccess.ReadWrite); 
            File.WriteAllBytes(fullPathAgain, uwb.downloadHandler.data);
            myAgainFile.Close(); */

            /* using (var Stream = File.Open(fullPath, FileMode.Create))
            {
                Debug.Log("stream is set");
                using (BinaryWriter binWriter = new BinaryWriter(Stream, Encoding.Default, false))
                {
                    Debug.Log("Writing the audio data to file.");

                    int arrLength = uwb.downloadHandler.data.Length;
                    binWriter.Write(uwb.downloadHandler.data, 0, arrLength);

                    byte[] verifier = new byte[arrLength];

                    using (BinaryReader binReader = new BinaryReader(binWriter.BaseStream))
                    {
                        binReader.BaseStream.Position = 0;

                        if (binReader.Read(verifier, 0, arrLength) != arrLength)
                        {
                            Debug.LogError("Error writing the data.");
                            return;
                        }
                    }
                }
            } */

            // Debug.Log($"{fileFullName} stored in {PATH_TO_AUDIO_FILES}. WARNING : server file will only be available for 24 hours !");
            getResponseObj = null; 
        }
    }
}

namespace CoreGameplayContent.VoiceLines
{
    [Serializable]
    internal class VoiceLines
    {
        [TextArea(10, 20)] public string text = "write here the text you want to generate speech for"; 
    }

    [Serializable]
    internal class POSTResponseObj
    {
        public string id;
        public string status;
        public string eta;
        public string text;
    }

    [Serializable]
    internal class GETResponseObj
    {
        public string id;
        public string status;
        public string url;
        public string job_time; 
    }
}
