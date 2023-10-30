using System;
using UnityEngine.Networking;

public class NotionWebRequest : WebRequestBase
{
    void Start()
    {
        StartCoroutine(WebRequestHandler.HandleRequest_GET(apiURL, OnGetRequestComplete));
    }

    protected override void Populate_OnGetComplete(UnityWebRequest uwb)
    {
        throw new NotImplementedException();
    }

    protected override void Populate_OnPostComplete(UnityWebRequest uwb)
    {
        throw new NotImplementedException();
    }
}
