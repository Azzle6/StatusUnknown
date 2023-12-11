using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using UnityEngine;
using XNode;

namespace StatusUnknown.Tools.Narrative
{
    [NodeWidth(400), NodeTint(80, 80, 10)]
    [CreateNodeMenu("Quest Node")]
    public class QuestNode : DialogueNode
    {
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private AccessType accessType;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private QuestSO quest; 

        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set"), PropertySpace] private QuestObjectSO newQuestObject;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set")] private QuestObjectSO newQuestReward;

        [Output] public DialogueLine result;  
        [ShowIf(nameof(GetIsValid))] public DialogueLine optionalDialogue;

        [PropertySpace(20), LabelWidth(LABEL_WIDTH_MEDIUM), HorizontalGroup(200, MarginLeft = 0.225f), Button("Refresh Self And Neighbour", ButtonSizes.Large)]
        public void RefreshOnQuestObjectChange() { RefreshOnValueChanged(); }

        private bool GetIsValid() => result.conditionIsValid && accessType == AccessType.Get;


        private void RefreshOnValueChanged()
        {
            SetDialogueLine();
        }

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
                Debug.Log("updating current quest with new objects and rewards");

                quest.OverrideQuestObject(newQuestObject);
                quest.OverrideQuestReward(newQuestReward);
            }
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
                    result.conditionIsValid = quest.AllQuestObjectAreRetrieved;
                    break;
                case AccessType.Set:
                    result.conditionIsValid = newQuestObject != null && newQuestReward != null;  
                    break;
                default: return default;
            }

            result.answer = optionalDialogue.answer; 

            return result;
        }
    }
}
