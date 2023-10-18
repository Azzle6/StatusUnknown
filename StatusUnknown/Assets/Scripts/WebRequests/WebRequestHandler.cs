using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class WebRequestHandler : MonoBehaviour
{
    public static IEnumerator HandleRequest(string apiUrl, Action<UnityWebRequest> callback = null)
    {
        using UnityWebRequest request = UnityWebRequest.Get(apiUrl);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success) { callback(request); }
        else
        {
            Debug.LogError("Web Request did not succeed");
        }
    }
}

