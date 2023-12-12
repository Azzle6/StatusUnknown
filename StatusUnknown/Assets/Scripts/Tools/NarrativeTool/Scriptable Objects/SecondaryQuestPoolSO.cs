using Aurore.DialogSystem;
using StatusUnknown.Utils.AssetManagement;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content.Narrative
{
    [ManageableData]
    [CreateAssetMenu(fileName = "Secondary Quest Pool", menuName = "Status Unknown/Narrative/Secondary Quest Pool")]
    public class SecondaryQuestPoolSO : QuestPoolSO
    {
        [SerializeField] private Dictionary<ReputationRank, (DialogGraph dialogue, QuestSO[] quests)> questPools;

        public override DialogGraph GetCurrentDialogue(ReputationRank key)
        {
            return questPools[key].dialogue;
        }

        public override QuestSO GetQuestFromPool(ReputationRank key)
        {
            return questPools[key].quests[Random.Range(0, questPools[key].quests.Length)];
        }
    }
}
