using Aurore.DialogSystem;
using StatusUnknown.Utils.AssetManagement;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    [CreateAssetMenu(fileName = "Main Quest Pool", menuName = "Status Unknown/Narrative/Main Quest Pool")]
    public class MainQuestsPoolSO : QuestPoolSO
    {

        [SerializeField] private Dictionary<ReputationRank, (DialogGraph dialogue, QuestSO quest)> questPools;

        public override DialogGraph GetCurrentDialogue(ReputationRank key)
        {
            return questPools[key].dialogue;
        }

        public override QuestSO GetQuestFromPool(ReputationRank key)
        {
            return questPools[key].quest;
        }
    }
}
