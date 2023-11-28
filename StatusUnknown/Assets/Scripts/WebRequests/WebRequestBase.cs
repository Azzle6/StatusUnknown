using StatusUnknown.Content.VoiceLines;
using System;
using UnityEngine;
using UnityEngine.Networking;

namespace StatusUnknown
{
    namespace WebRequest
    {
        /// <summary>
        /// This is the base class for all type of web requests. Supposed to be derived from your custom WebRequest classes.
        /// </summary>

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

            /// <summary>
            /// Inspired by RestSharp. Automatically handles ? and = for query params
            /// </summary>
            /// <param name="key"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            protected string AddQueryParam(string key, string value) => string.Concat("?", key, "=", value); 
        }
    }
}
