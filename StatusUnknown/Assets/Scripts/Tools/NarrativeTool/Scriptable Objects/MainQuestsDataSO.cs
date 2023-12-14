using Aurore.DialogSystem;
using StatusUnknown.Utils.AssetManagement;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    [CreateAssetMenu(fileName = "Main Quest Pool", menuName = CoreContentStrings.PATH_CONTENT_NARRATIVE + "Main Quest Pool")]
    public class MainQuestsDataSO : QuestDataSO
    {

        [SerializeField] private Dictionary<ReputationRank, (DialogGraph dialogue, QuestSO quest)> questPools;

        public override DialogGraph GetCurrentDialogue(ReputationRank key)
        {
            return questPools[key].dialogue;
        }

        public override QuestSO GetQuest(ReputationRank key)
        {
            return questPools[key].quest;
        }
    }
}
