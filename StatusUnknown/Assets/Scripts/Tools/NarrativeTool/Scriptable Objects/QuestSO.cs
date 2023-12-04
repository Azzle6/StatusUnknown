using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [CreateAssetMenu(fileName = "Quest", menuName = "Status Unknown/Narrative/Quest")]
    public class QuestSO : ScriptableObject
    {
        [SerializeField] private string questName;
        [SerializeField] private Faction factionQuestGiver;
        [SerializeField] private QuestObjectSO questObjectToRetrieve;
        [SerializeField] private QuestObjectSO questReward;
        [SerializeField, Range(0, 1000)] private int reputationCompletionBonus = 10; 

        [field:SerializeField] public bool QuestObjectIsRetrieved { get; set; }
        public int ReputationCompletionBonus { get => reputationCompletionBonus; }

        public int QuestIndex { get; set; } 
        public QuestObjectSO QuestObjectToRetrieve { get => questObjectToRetrieve; private set => questObjectToRetrieve = value; }
        public QuestObjectSO QuestReward { get => questReward; private set => questReward = value; }
        public Faction FactionQuestGiver { get => factionQuestGiver; }

        public void OverrideQuestObject(QuestObjectSO newQuestObject)
        {
            QuestObjectToRetrieve = newQuestObject; 
        }

        public void OverrideQuestReward(QuestObjectSO newQuestReward)
        {
            questReward = newQuestReward;
        }
    }
}
