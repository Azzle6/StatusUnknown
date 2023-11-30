using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Status Unknown/Narrative/Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private string questGiver;
        [SerializeField] private QuestObjectSO questObjectToRetrieve;
        [SerializeField] private QuestObjectSO questReward;

        [field:SerializeField] public bool QuestObjectIsRetrieved { get; set; }

        public int QuestIndex { get; set; } 
        public QuestObjectSO QuestObjectToRetrieve { get => questObjectToRetrieve; }
        public QuestObjectSO QuestReward { get => questReward; }
    }
}
