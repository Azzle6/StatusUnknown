using HoudiniEngineUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
{
    public static IEnumerator HandleRequest_POST(string apiUrl, string postData, Dictionary<string, string> headers = null, Action<UnityWebRequest> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Post(apiUrl, postData, "application/json");
        foreach (KeyValuePair<string, string> item in headers)
        {
            request.SetRequestHeader(item.Key, item.Value);
        }
       
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) { callback(request); }
        else
        {
            Debug.LogError($"POST method did not succeed. Error : {request.error}");
        } 
    }

    public static IEnumerator HandleRequest_GET(string apiUrl)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = Encoding.UTF8.GetString(request.downloadHandler.data);
                Debug.Log($"result : {jsonResult}");
            }
            else
            {
                Debug.LogError($"GET method did not succeed. Error : {request.error}");
            }
        }
    }

    public static IEnumerator HandleRequest_GET(string apiUrl, Action<UnityWebRequest> callback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                string jsonResult = Encoding.UTF8.GetString(request.downloadHandler.data);
                Debug.Log($"result : {jsonResult}");

                callback(request);
            }
            else
            {
                Debug.LogError($"GET method did not succeed. Error : {request.error}");
            }
        }
    }

    public static IEnumerator HandleRequest_GET(string apiUrl, Dictionary<string, string> headers = null, string getData = null, Action < UnityWebRequest> callback = null)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(apiUrl))
        {
            foreach (KeyValuePair<string, string> item in headers)
            {
                request.SetRequestHeader(item.Key, item.Value);
            }

            request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(getData));

            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) 
            {
                string jsonResult = Encoding.UTF8.GetString(request.downloadHandler.data);
                Debug.Log($"result : {jsonResult}"); 

                callback(request); 
            }
            else
            {
                Debug.LogError($"GET method did not succeed. Error : {request.error}");
            }
        }
    }
}

