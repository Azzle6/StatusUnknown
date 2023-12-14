using Aurore.DialogSystem;
using Sirenix.OdinInspector;

namespace StatusUnknown.Content.Narrative
{
    public abstract class QuestDataSO : SerializedScriptableObject
    {
        // called from node SET
        public abstract QuestSO GetQuest(ReputationRank key);
        public abstract DialogGraph GetCurrentDialogue(ReputationRank key); 
    }
}
