using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;
using StatusUnknown.CoreGameplayContent.VoiceLines;

// api URL https://rapidapi.com/k_1/api/large-text-to-speech
// app URL https://rapidapi.com/developer/analytics/Status-Unknown-TTS 
// uri GET ?id=...
// auth key "..." (NOT DEPLOYED)

namespace StatusUnknown
{
    namespace WebRequest
    {
        public enum AudioFileExtension { wav, mp3, ogg }
        public enum DialogueType { Test, NPC, Character }

        /// <summary>
        /// Web Request class to generate text from speech. Uses https://rapidapi.com/k_1/api/large-text-to-speech as web api. 
        /// Files are to be downloaded from an amazon server by providing an url.
        /// </summary>
        public class TTSWebRequest : WebRequestBase
        {
            [SerializeField] private VoiceLines voiceLinesToPost;
            [SerializeField] private string fileName = "my-file-name";
            [SerializeField] private AudioFileExtension fileExtension = AudioFileExtension.wav;
            [SerializeField] private DialogueType dialogueType = DialogueType.Test;

            private string fileFullName;
            private Dictionary<string, string> headers = new Dictionary<string, string>();
            private string getURI;
            private const string PATH_TO_AUDIO_FILES = "/Audio/Files/";


            [Space, Header("-- DEBUG --")]
            [SerializeField] private AudioSource audioSource;
            [SerializeField] private GETResponse getResponseSO;
            private POSTResponseObj postResponseObj = new POSTResponseObj();
            private GETResponseObj getResponseObj = new GETResponseObj();

            private void Start()
            {
                fileFullName = string.Concat("SU_", dialogueType, "_", fileName, ".", fileExtension);

                if (useScriptableIfProvided && !string.IsNullOrEmpty(getResponseSO.ID))
                {
                    if (getResponseSO == null)
                    {
                        Debug.LogError("getResponseSO is not provided");
                        return;
                    }
                    if (string.IsNullOrEmpty(getResponseSO.ID))
                    {
                        Debug.LogError("getResponseSO id is empty, GET request cannot be done");
                        return;
                    }

                    Debug.Log("using ");

                    postResponseObj.id = getResponseSO.ID;

                    OnGetRequestComplete(new UnityWebRequest()); // BAD solution. Allocating on the heap for no reason
                    return;
                }

                PostRequest();
            }

            // [ContextMenu("Generate TTS file")]
            private void PostRequest()
            {
                Debug.Log("doing POST");

                headers = new Dictionary<string, string>
        {
            { "Content-Type", "application/json" },
            { "x-rapidapi-host", "large-text-to-speech.p.rapidapi.com" },
            { "x-rapidapi-key", $"{authKey}" }
        };

                getResponseSO.SetText(voiceLinesToPost.text);

                string postData = JsonUtility.ToJson(voiceLinesToPost);
                StartCoroutine(WebRequestHandler.HandleRequest_POST(apiURL, postData, headers, OnPostRequestComplete));
            }

            protected override void Populate_OnPostComplete(UnityWebRequest uwb)
            {
                postResponseObj = JsonUtility.FromJson<POSTResponseObj>(uwb.downloadHandler.text);
                postResponseObj.id = string.Concat("?id=", postResponseObj.id);

                getResponseSO.ID = postResponseObj.id;

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
                getResponseSO.Uri = getURI;

                StartCoroutine(WebRequestHandler.HandleRequest_GET(getURI, headers, OnGetRequestComplete));
            }

            protected override void Populate_OnGetComplete(UnityWebRequest uwb)
            {
                if (string.IsNullOrEmpty(getResponseObj.url))
                {
                    if (debugGetMessage && uwb.downloadHandler != null)
                    {
                        Debug.Log($"GET response : {uwb.downloadHandler.text}");
                    }

                    if (useScriptableIfProvided)
                    {
                        Debug.Log("getting data from scriptable object");
                        getResponseObj.url = getResponseSO.Url;
                    }
                    else
                    {
                        getResponseObj = JsonUtility.FromJson<GETResponseObj>(uwb.downloadHandler.text);

                        if (getResponseSO && string.IsNullOrEmpty(getResponseSO.Url))
                        {
                            getResponseSO.Url = getResponseObj.url;
                        }
                    }

                    StartCoroutine(WebRequestHandler.HandleRequest_GET_MEDIA(getResponseObj.url, OnGetRequestComplete));
                }
                else
                {
                    DownloadHandlerAudioClip dlHandler = uwb.downloadHandler as DownloadHandlerAudioClip;

                    AudioClip _clip = dlHandler?.audioClip;
                    _clip.name = fileName;
                    audioSource.PlayOneShot(_clip);

                    string fullPath = string.Concat(Application.dataPath, PATH_TO_AUDIO_FILES, fileFullName);

                    using (var Stream = File.Open(fullPath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                    {
                        using (BinaryWriter binWriter = new BinaryWriter(Stream, Encoding.Default, false))
                        {
                            int arrLength = dlHandler.data.Length;
                            binWriter.Write(dlHandler.data, 0, arrLength);

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

                            Debug.Log($"{fileFullName} stored in {PATH_TO_AUDIO_FILES}. WARNING : server file will only be available for 24 hours !");
                        }
                    }

                    getResponseObj = null;
                }
            }
        }
    }

    // this maybe be added to it's own separate class
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
}
