using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
{
    public static IEnumerator HandleRequest_POST(string apiUrl, string postData, Dictionary<string, string> headers = null, Action<UnityWebRequest> callback = null)
    {
        // string postData = JsonUtility.ToJson(textField);

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

    public static IEnumerator HandleRequest_GET(string apiUrl, Action<UnityWebRequest> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) { callback(request); }
        else
        {
            Debug.LogError($"GET method did not succeed. Error : {request.error}");
        }
    }
}

