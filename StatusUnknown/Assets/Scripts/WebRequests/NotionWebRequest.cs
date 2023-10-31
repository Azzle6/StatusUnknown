using System;
using UnityEngine.Networking;

public class NotionWebRequest : WebRequestBase
{
    void Start()
    {

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
