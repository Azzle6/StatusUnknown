using UnityEngine;

namespace StatusUnknown
{
    namespace WebRequest
    {
        /// <summary>
        /// Scriptable Object to store the last Json response from TTS server, in order to avoid redoing nedless POST requests and directly doing GET
        /// </summary>
        [CreateAssetMenu(fileName = "GET Json Object", menuName = "StatusUnknown/Web/TTS/Response")]
        public class GETResponse : ScriptableObject
        {
            public string ID { get; set; }
            public string Url { get; set; }
            public string Uri { get; set; }

            [Space, TextArea(10, 20)] private string Text;

            public void SetText(string txt)
            {
                Text = txt;
            }

            public string GetText() => Text;
        }
    }
}

