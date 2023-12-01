using Sirenix.OdinInspector;
using System;
using UnityEngine;
using XNode;

namespace StatusUnknown.Tools.Narrative
{
    // -- COMPARER --
    // VALUE SMALLER/EQUAL/GREATER THAN 
    // HAS OBJECT(S)

    [NodeWidth(400), NodeTint(120, 0, 150)]
    [CreateNodeMenu("Conditional Answer Node")]
    public class DialogueConditionalAnswerNode : Node
    {
        [HideIf("@" + nameof(comparisonType) + " == ComparisonType.QuestIsDone"), LabelWidth(LABEL_WIDTH_MEDIUM), OnValueChanged(nameof(RefreshOnValueChanged))]
        public int source;

        [HideIf("@" + nameof(comparisonType) + " == ComparisonType.QuestIsDone"), LabelWidth(LABEL_WIDTH_MEDIUM), OnValueChanged(nameof(RefreshOnValueChanged))]
        public int target;

        [ShowIf("@" + nameof(comparisonType) + " == ComparisonType.QuestIsDone"), LabelWidth(LABEL_WIDTH_MEDIUM)]
        [Input] public Data input;  

        [ShowIf("@comparisonType == ComparisonType.QuestIsDone"), LabelWidth(LABEL_WIDTH_MEDIUM), HorizontalGroup(200, MarginLeft = 0.225f), Button("Refresh Self And Neighbour", ButtonSizes.Large)]
        public void refreshOnQuestObjectChange() { RefreshOnValueChanged(); } // bootleg solution because I don't know how to directly track a Reference change, only a value change

        public enum ComparisonType { SmallerThan, SmallerThanOrEqual, GreaterThan, GreaterThanOrEqual, Equal, NotEqual, QuestIsDone }
        [LabelWidth(LABEL_WIDTH_MEDIUM), OnValueChanged(nameof(RefreshOnValueChanged))] public ComparisonType comparisonType = ComparisonType.SmallerThan;

        [PropertySpace(spaceBefore: 50), Output] public Data result;

        private int previousValue_Source;
        private int previousValue_Target;

        NodePort output;

        protected override void Init()
        {
            base.Init();
            output = GetOutputPort("result");

            RefreshOnValueChanged(); 
        }

        // bootleg solution. Need to find a cleaner way to track value change an callback (not using this OnRemoveConnection to refresh neighbour port.value)
        // ChangeEvent<T>.GetPooled() and INotifyValueChanged<T> did not work here..
        private void RefreshOnValueChanged()
        {
            GetValue(output);

            previousValue_Source = source;
            previousValue_Target = target;

            if (output.Connection == null) return; 
            output.Connection.node.OnRemoveConnection(output);
        }

        public override object GetValue(NodePort port)
        {
            float source = GetInputValue<float>("source", this.source);
            float target = GetInputValue<float>("target", this.target);
            input = GetInputValue("input", new Data());

            if (port.fieldName == "result")
                result.isValid = comparisonType switch
                {
                    ComparisonType.SmallerThanOrEqual => source <= target,
                    ComparisonType.GreaterThan => source > target,
                    ComparisonType.GreaterThanOrEqual => source >= target,
                    ComparisonType.Equal => source == target,
                    ComparisonType.NotEqual => source != target,
                    ComparisonType.QuestIsDone => input.isValid,
                    _ => false
                };

            result.dialogue = input.dialogue;
            return result;
        }

        // TODO : update onValueChange (source/target)
        // TODO : notify output node that value changed
    }
}
