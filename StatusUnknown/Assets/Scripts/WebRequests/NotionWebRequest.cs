using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class NotionWebRequest : WebRequestBase
{
    void Start()
    {
        StartCoroutine(WebRequestHandler.HandleRequest(apiURL, OnRequestComplete));
    }

    protected override void Populate(UnityWebRequest uwb)
    {
        Debug.Log("REQUEST DONE"); 
    }
}
