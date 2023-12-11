using Sirenix.OdinInspector;
using UnityEngine;
using XNode;
using StatusUnknown.Content;

namespace StatusUnknown.Tools.Narrative
{
    // Ideally, this node displays all the serialized property of the playerSO and dynamically SET/GET the proper value
    // this will need some leg-work in terms of reflection

    // FOR NOW : just get/set on player reputation

    [NodeWidth(400), NodeTint(100, 20, 20)]
    [CreateNodeMenu("Faction Reputation Node")]
    public class FactionReputationNode : DialogueNode
    {
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private AccessType accessType;

        [PropertySpace, SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private PlayerDataSO playerData;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM)] private Faction Faction;
        [SerializeField, LabelWidth(LABEL_WIDTH_MEDIUM), ShowIf("@accessType == AccessType.Set")] private int valueToAdd;

        [Output, ShowIf("@accessType == AccessType.Get")] object result; 

        public override void OnEnter()
        {
            base.OnEnter();

            switch (accessType)
            {
                case AccessType.Get:
                    result = playerData.GetCurrentReputation(Faction);
                    break;
                case AccessType.Set:
                    playerData.UpdateCurrentReputation(valueToAdd, Faction);
                    result = null;
                break;
            }
        }

        public override object GetValue(NodePort port)
        {
            return result;
        }
    }
}
