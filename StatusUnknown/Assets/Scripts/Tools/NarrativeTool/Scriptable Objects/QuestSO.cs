using StatusUnknown.Utils.AssetManagement;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    public abstract class QuestSO : ScriptableObject
    {
        [SerializeField] protected string questName;
        [SerializeField, TextArea(5,10)] protected string description;
        [SerializeField] protected Faction factionQuestGiver;
        [SerializeField] protected QuestObjectSO[] questObjectToRetrieve;
        [SerializeField] protected QuestObjectSO[] questReward;
        [SerializeField, Range(0, 1000)] protected int reputationCompletionBonus = 10; 

        [field:SerializeField] public virtual bool AllQuestObjectAreRetrieved { get; set; }
        public virtual int ReputationCompletionBonus { get => reputationCompletionBonus; }

        public virtual int QuestIndex { get; set; } 
        public virtual QuestObjectSO[] QuestObjectsToRetrieve { get => questObjectToRetrieve; private set => questObjectToRetrieve = value; }
        public virtual QuestObjectSO[] QuestRewards { get => questReward; private set => questReward = value; }
        public virtual Faction FactionQuestGiver { get => factionQuestGiver; }

        public virtual void OverrideQuestObject(QuestObjectSO newQuestObject, int index = 0)
        {
            QuestObjectsToRetrieve[index] = newQuestObject; 
        }

        public virtual void OverrideQuestReward(QuestObjectSO newQuestReward, int index = 0)
        {
            questReward[index] = newQuestReward;
        }
    }
}
