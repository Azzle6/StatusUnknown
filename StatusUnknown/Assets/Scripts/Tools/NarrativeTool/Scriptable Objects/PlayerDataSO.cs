using Sirenix.OdinInspector;
using StatusUnknown.Content.Narrative;
using StatusUnknown.Utils.AssetManagement;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.Content
{
    public enum ReputationRank { Zero, One, Two, Three }

    [ManageableData]
    [CreateAssetMenu(fileName = "Player Data", menuName = "Status Unknown/Gameplay/Player Data")]
    public class PlayerDataSO : SerializedScriptableObject
    {
        public enum ReputationMechanic { Simple, AllowMultiRanking }

        [field: SerializeField] public QuestJournalSO QuestJournal { get; private set; }

        private Action<int, Faction, QuestObjectSO[]> OnQuestCompletion;
        [field: SerializeField] private List<QuestObjectSO> CompletedQuestRewards { get; set; }

        public class RankData
        {
            [LabelWidth(200)] public int currentReputationValue = 0;
            [LabelWidth(200)] public ReputationRank currentReputationRank;
            [LabelWidth(200)] public ReputationMechanic reputationMechanic;
            private int[] ReputationCeils;
            private const int RANK_MAX = 3; // temp
            bool canUnlockNextFactionQuest, hittingRankBottomValue;
            public bool CanUnlockNextFactionQuest { get => canUnlockNextFactionQuest; }

            // This system must be kept independant from the quest, because other elements may upgrade the player reputation (for ex: dialogue options)
            public void UpdateReputationRank(int additionalReputation, int[] reputationCeils)
            {
                ReputationCeils = new int[reputationCeils.Length];
                Array.Copy(reputationCeils, ReputationCeils, reputationCeils.Length);

                currentReputationValue += additionalReputation;

                switch (reputationMechanic)
                {
                    case ReputationMechanic.Simple:
                        CalculateReputation_Simple(additionalReputation, reputationCeils);
                        break;
                    case ReputationMechanic.AllowMultiRanking:
                        CalculateReputation_Multi(additionalReputation, reputationCeils);
                        break;
                }
            }

            private void CalculateReputation_Simple(int additionalReputation, int[] reputationCeils)
            {
                canUnlockNextFactionQuest = currentReputationValue - reputationCeils[(int)currentReputationRank + 1] >= 0;
                hittingRankBottomValue = Math.Sign(currentReputationValue) < 0;

                int newRankToInt = (int)currentReputationRank + (hittingRankBottomValue ? -1 : 1);

                if (canUnlockNextFactionQuest)
                {
                    // rank upgrade
                    //UpgradeToNewRank(newRankToInt);
                    currentReputationValue = ReputationCeils[(int)currentReputationRank];

                    Debug.Log($"New faction quest is available to gain rank : {newRankToInt}");
                }
                else if (hittingRankBottomValue)
                {
                    currentReputationValue = 0;
                    Debug.Log("reached bottom of rank xp");
                }
            }

            private void UpgradeToNewRank(int newRankToInt)
            {
                currentReputationRank = (ReputationRank)Math.Clamp(Math.Min(newRankToInt, RANK_MAX), 0, RANK_MAX);

                // remainder is kept unless you hit max rank (no ghost leveling)
                if (currentReputationRank == (ReputationRank)RANK_MAX)
                {
                    Debug.Log("max rank was hit");
                    currentReputationValue = 0;
                    return;
                }
            }


            // allows for multi ranking. May be removed in the future
            private void CalculateReputation_Multi(int additionalReputation, int[] reputationCeils)
            {
                do
                {
                    canUnlockNextFactionQuest = currentReputationValue - reputationCeils[(int)currentReputationRank + 1] >= 0;
                    hittingRankBottomValue = Math.Sign(additionalReputation) < 0;

                    int newRankToInt = (int)currentReputationRank + (hittingRankBottomValue ? -1 : 1);

                    // nothing more to do if just changing reputation value but not rank
                    if (hittingRankBottomValue || canUnlockNextFactionQuest)
                    {
                        // rank upgrade
                        currentReputationRank = (ReputationRank)Math.Clamp(Math.Min(newRankToInt, RANK_MAX), 0, RANK_MAX);

                        // carrry over
                        if (canUnlockNextFactionQuest)
                        {
                            currentReputationValue -= ReputationCeils[(int)currentReputationRank];
                        }
                        else
                        {
                            currentReputationValue = newRankToInt < 0 ?
                                0 : // can't go into a negative value if you are at bottom of rank zero
                                currentReputationValue + ReputationCeils[(int)currentReputationRank + 1]; // negative + positive = missin reputation to reach rank again
                        }

                        Debug.Log($"New rank : {currentReputationRank} \n With remainder xp : {currentReputationValue}");

                        // remainder is kept unless you hit max rank (no ghost leveling)
                        if (currentReputationRank == (ReputationRank)RANK_MAX)
                        {
                            Debug.Log("max rank was hit");
                            currentReputationValue = 0;
                            return;
                        }
                    }
                } while (canUnlockNextFactionQuest || hittingRankBottomValue);
            }
        }

        public Dictionary<Faction, RankData> rankDatas = new Dictionary<Faction, RankData>();

        private readonly List<int[]> reputationCeils = new List<int[]>()
    {
        new int[] { 0, 100, 400, 800 },
        new int[] { 0, 300, 600, 1000 },
        new int[] { 0, 200, 500, 750 },
        new int[] { 0, 150, 450, 1200 },
    }; /* temporary gameplay data storage       SAA, 
                                                Hera, 
                                                Excelsior, 
                                                Pulse
    */


        private void OnEnable()
        {
            OnQuestCompletion += UpdateCurrentReputation;
        }

        private void OnDisable()
        {
            OnQuestCompletion -= UpdateCurrentReputation;
        }

        public void Init()
        {
            QuestJournal.Init(OnQuestCompletion);
        }

        public void UpdateCurrentReputation(int additionalReputation, Faction key, QuestObjectSO[] questReward = null)
        {
            // DO NOT modify the player rank if the quest was already completed
            rankDatas[key].UpdateReputationRank(additionalReputation, reputationCeils[(int)key]);

            if (questReward == null || questReward.Length == 0) return;

            for (int i = 0; i < questReward.Length; i++)
            {
                var currentCachedReward = questReward[i];
                if (!CompletedQuestRewards.Contains(currentCachedReward))
                {
                    CompletedQuestRewards.Add(currentCachedReward);
                }
            }
        }

        public (int, ReputationRank) GetCurrentReputation(Faction key)
        {
            RankData rd = rankDatas[key];
            (int xpRemainder, ReputationRank reputationRank) returnValue = new(rd.currentReputationValue, rd.currentReputationRank);

            return returnValue;
        }

        public (bool, int) CanUnlockFactionMainQuest(Faction key)
        {
            return (rankDatas[key].CanUnlockNextFactionQuest, (int)rankDatas[key].currentReputationRank + 1);
        }
    }
}

