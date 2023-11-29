using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Status Unknown/Narrative/Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string questGiver;
        [SerializeField] private QuestObjectSO questObjectToRetrieve;

        public int QuestIndex { get; set; } 
    }
}
