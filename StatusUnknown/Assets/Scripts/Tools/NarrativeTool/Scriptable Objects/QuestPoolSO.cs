using Aurore.DialogSystem;
using Sirenix.OdinInspector;

namespace StatusUnknown.Content.Narrative
{
    public abstract class QuestPoolSO : SerializedScriptableObject
    {
        // called from node SET
        public abstract QuestSO GetQuestFromPool(ReputationRank key);
        public abstract DialogGraph GetCurrentDialogue(ReputationRank key); 
    }
}
