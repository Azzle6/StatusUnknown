using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using UnityEngine;
using XNode;

namespace StatusUnknown.Tools.Narrative
{
    [NodeWidth(400), NodeTint(80, 80, 10)]
    [CreateNodeMenu("Quest Node")]
    public class QuestNode : Node
    {
        [SerializeField] AccessType accessType;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Get")] private QuestSO quest;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set")] private QuestObjectSO questObject;

        [Output] public DialogueLine result;  
        [ShowIf(nameof(GetIsValid))] public DialogueLine optionalDialogue;

        [PropertySpace(20), LabelWidth(LABEL_WIDTH_MEDIUM), HorizontalGroup(200, MarginLeft = 0.225f), Button("Refresh Self And Neighbour", ButtonSizes.Large)]
        public void refreshOnQuestObjectChange() { RefreshOnValueChanged(); }

        private bool GetIsValid() => result.isValid;

        private void RefreshOnValueChanged()
        {
            SetDialogueLine();
        }

        protected override void Init()
        {
            base.Init();
            SetDialogueLine();
        }

        public override object GetValue(NodePort port)
        {
            return SetDialogueLine(); 
        }

        private DialogueLine SetDialogueLine()
        {
            result = new DialogueLine();    
            switch (accessType)
            {
                case AccessType.Get:
                    result.isValid = quest.QuestObjectIsRetrieved;
                    break;
                case AccessType.Set:
                    result.isValid = questObject != null;
                    break;
                default: return default;
            }

            result.answer = optionalDialogue.answer; 

            return result;
        }
    }
}
