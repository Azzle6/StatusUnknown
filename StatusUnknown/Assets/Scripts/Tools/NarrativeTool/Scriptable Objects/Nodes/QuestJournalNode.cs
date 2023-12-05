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
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private AccessType accessType;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private QuestJournalSO playerQuestJournal; 
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set")] private QuestSO questToAdd; 
        [Output] public ScriptableObject result;

        protected override void Init()
        {
            base.Init();
            if (accessType == AccessType.Get)
                execIn = new State();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (accessType == AccessType.Set)
            {
                Debug.Log("adding new quest to journal");

                playerQuestJournal.AddQuest(questToAdd);
            }
        }

        public override object GetValue(NodePort port)
        {
            switch (accessType)
            {
                case AccessType.Get:
                    result = playerQuestJournal;
                    break;
                case AccessType.Set:
                    result = questToAdd;
                    break;
                default: return null;
            }

            return result;
        }
    }
}

