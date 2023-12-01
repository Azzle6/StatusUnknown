using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Dialogue Line", menuName = "Status Unknown/Narrative/Dialogue Line")]
    public class DialogueLineSO : SerializedScriptableObject
    {
        public (string dialogueLine, int ID) IntStringMap;
        public bool triggersGameplayEffect; 
    }
}
