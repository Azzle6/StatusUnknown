using Aurore.DialogSystem;
using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using UnityEngine;
using XNode;

[NodeWidth(400), NodeTint(120, 120, 0)]
[CreateNodeMenu("Quest Giver Node")]
public class QuestNode : DialogueNode
{
    [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private QuestSO givenQuest;

    public override object GetValue(NodePort port)
    {
        return givenQuest;
    }
}
