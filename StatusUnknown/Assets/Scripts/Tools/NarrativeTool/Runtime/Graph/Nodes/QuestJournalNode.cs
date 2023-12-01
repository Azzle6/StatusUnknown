using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using UnityEngine;
using XNode;

namespace StatusUnknown.Tools.Narrative
{
    [NodeWidth(400), NodeTint(120, 120, 0)]
    [CreateNodeMenu("Quest Journal Node")]
    public class QuestJournalNode : DialogueNode
    {
        [SerializeField] AccessType accessType;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Get")] private QuestJournalSO playerQuestJournal; 
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set")] private QuestSO questToGive;
        [Output] public ScriptableObject result;

        public override object GetValue(NodePort port)
        {
            switch(accessType)
            {
                case AccessType.Get: 
                    result = playerQuestJournal; 
                    break;
                case AccessType.Set:
                    result = questToGive;
                    break;
                default: return null;
            }

            return result;
        }
    }
}

