using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Status Unknown/Narrative/Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] private string questName = string.Empty;
        [SerializeField] private string questGiver = string.Empty;
        public int QuestIndex { get; set; } 
    }
}
