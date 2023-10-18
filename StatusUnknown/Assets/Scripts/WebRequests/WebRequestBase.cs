using System;
using UnityEngine;
using UnityEngine.Networking;

public abstract class WebRequestBase : MonoBehaviour
{
    [SerializeField] protected string apiURL;
    [SerializeField, Range(0, 10)] protected int amountOfRequests = 3;

    protected abstract void Populate(UnityWebRequest uwb);
    protected Action<UnityWebRequest> OnRequestComplete;


    protected void OnEnable()
    {
        OnRequestComplete += Populate;
        Debug.Log("enabling"); 
    }

    protected void OnDisable()
    {
        OnRequestComplete -= Populate;
        Debug.Log("disabling"); 
    }
}
