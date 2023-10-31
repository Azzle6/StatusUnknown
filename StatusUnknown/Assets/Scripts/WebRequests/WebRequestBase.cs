using System;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WebRequestBase : MonoBehaviour
{
    [SerializeField] protected string apiURL;
    [SerializeField] protected string authKey = ""; 
    [SerializeField, Range(1, 10)] protected int amountOfRequests = 1;

    [Header("-- DEBUG --")]
    [SerializeField] protected bool useScriptableIfProvided = true;
    [SerializeField] protected bool debugPostMessage = true;
    [SerializeField] protected bool debugGetMessage = true; 

    protected abstract void Populate_OnPostComplete(UnityWebRequest uwb);
    protected abstract void Populate_OnGetComplete(UnityWebRequest uwb);
    protected Action<UnityWebRequest> OnGetRequestComplete;
    protected Action<UnityWebRequest> OnPostRequestComplete; 


    protected void OnEnable()
    {
        OnPostRequestComplete += Populate_OnPostComplete;
        OnGetRequestComplete += Populate_OnGetComplete;
    }

    protected void OnDisable()
    {
        OnPostRequestComplete -= Populate_OnPostComplete;
        OnGetRequestComplete -= Populate_OnGetComplete;
    }
}
