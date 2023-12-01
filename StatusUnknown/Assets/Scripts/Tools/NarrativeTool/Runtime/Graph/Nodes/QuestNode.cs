using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using System;
using UnityEngine;
using XNode;

namespace StatusUnknown.Tools.Narrative
{
    [Serializable]
    public struct Data
    {
        public string dialogue;
        public bool isValid;
    }

    [NodeWidth(400), NodeTint(80, 80, 10)]
    [CreateNodeMenu("Quest Node")]
    public class QuestNode : Node
    {
        [SerializeField] AccessType accessType;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Get")] private QuestSO quest;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set")] private QuestObjectSO questObject;

        [Output] public Data result;  
        [TextArea, ShowIf(nameof(GetIsValid))] public string optionalDialogue = string.Empty;

        [PropertySpace(20), LabelWidth(LABEL_WIDTH_MEDIUM), HorizontalGroup(200, MarginLeft = 0.225f), Button("Refresh Self And Neighbour", ButtonSizes.Large)]
        public void refreshOnQuestObjectChange() { RefreshOnValueChanged(); }

        private bool GetIsValid() => result.isValid;

        private void RefreshOnValueChanged()
        {
            SetData();
        }

        protected override void Init()
        {
            base.Init();
            SetData();
        }

        public override object GetValue(NodePort port)
        {
            return SetData(); 
        }

        private Data SetData()
        {
            result = new Data();    
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

            result.dialogue = optionalDialogue; 

            return result;
        }
    }
}
