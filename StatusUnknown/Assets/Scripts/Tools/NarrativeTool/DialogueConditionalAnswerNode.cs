using Aurore.DialogSystem;
using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using UnityEngine;
using XNode;

// -- COMPARER --
// VALUE SMALLER/EQUAL/GREATER THAN 
// HAS OBJECT(S)

[NodeWidth(400), NodeTint(120, 0, 150)]
[CreateNodeMenu("Conditional Answer Node")]
public class DialogueConditionalAnswerNode : DialogueNode
{
    [Input, HideIf("@" + nameof(comparisonType) + " == ComparisonType.HasQuestObjects"), LabelWidth(LABEL_WIDTH_MEDIUM)] public int source;
    [Input, HideIf("@" + nameof(comparisonType) + " == ComparisonType.HasQuestObjects"), LabelWidth(LABEL_WIDTH_MEDIUM)] public int target;
    [Input, ShowIf("@" + nameof(comparisonType) + " == ComparisonType.HasQuestObjects"), LabelWidth(LABEL_WIDTH_MEDIUM)] public QuestObjectSO ownedQuestObject; 

    [ShowIf("@" + nameof(comparisonType) + " == ComparisonType.HasQuestObjects"), LabelWidth(LABEL_WIDTH_MEDIUM)] public QuestObjectSO requiredQuestObject;

    public enum ComparisonType { SmallerThan, SmallerThanOrEqual, GreaterThan, GreaterThanOrEqual, Equal, NotEqual, HasQuestObjects }
    [LabelWidth(LABEL_WIDTH_MEDIUM)] public ComparisonType comparisonType = ComparisonType.SmallerThan;

    [PropertySpace(spaceBefore:50), Output] public bool result;

    public override object GetValue(NodePort port)
    {
        float source = GetInputValue<float>("source", this.source);
        float target = GetInputValue<float>("target", this.target);

        if (port.fieldName == nameof(result))
            result = comparisonType switch
            {
                ComparisonType.SmallerThanOrEqual => source <= target,
                ComparisonType.GreaterThan => source > target,
                ComparisonType.GreaterThanOrEqual => source >= target,
                ComparisonType.Equal => source == target,
                ComparisonType.NotEqual => source != target,
                ComparisonType.HasQuestObjects => Equals(ownedQuestObject, requiredQuestObject),
                _ => false,
            }; 
        return result;
    }
}
