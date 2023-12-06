using Sirenix.OdinInspector;
using UnityEngine;

namespace StatusUnknown
{
    namespace WebRequest
    {
        /// <summary>
        /// Scriptable Object to store the last Json response from TTS server, in order to avoid redoing nedless POST requests and directly doing GET
        /// </summary>
        [CreateAssetMenu(fileName = "GET Json Object", menuName = "Status Unknown/Web/TTS/Response")]
        public class GETResponse : ScriptableObject
        {
            private const bool ENABLE = false; 
            [field:SerializeField, EnableIf("ENABLE")] public string ID { get; set; }
            [field: SerializeField, EnableIf("ENABLE")] public string Url { get; set; }
            [field: SerializeField, EnableIf("ENABLE")] public string Uri { get; set; }

            [Space, TextArea(10, 20), EnableIf("ENABLE")] public string Text;

            public void SetText(string txt)
            {
                Text = txt;
            }

            public string GetText() => Text;
        }
    }
}

