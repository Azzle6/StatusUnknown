using Sirenix.OdinInspector;
using StatusUnknown.Tools;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Dialogue", menuName = CoreContentStrings.PATH_CONTENT_NARRATIVE + "Dialogue")]
    public class DialogueSO : SerializedScriptableObject
    {
        [SerializeField]
        private Dictionary<Faction, string> dialogueLine = new Dictionary<Faction, string>()
            {
                { Faction.Hera, "We are Hera, and we will rule." },
                { Faction.SAA, "Yeah... You mad bro ?" },
                { Faction.Pulse, "LET'S DESTROY EVERYTHING !!!" },
            };
    }
}
