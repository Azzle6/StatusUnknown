using Sirenix.OdinInspector;
using Sirenix.Utilities;
using StatusUnknown;
using StatusUnknown.Content.Narrative;
using StatusUnknown.Utils.AssetManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ManageableData]
[CreateAssetMenu(fileName = "Player Data", menuName = "Status Unknown/Gameplay/Player Data")]
public class PlayerDataSO : SerializedScriptableObject
{
    public enum ReputationRank { Zero, One, Two, Three }
    [field: SerializeField] public QuestJournalSO QuestJournal { get; private set; }

    private Action<int, Faction, QuestObjectSO[]> OnQuestCompletion;
    [field: SerializeField] private List<QuestObjectSO> CompletedQuestRewards { get; set; }

    public class RankData
    {
        [LabelWidth(200)] public int currentReputationValue = 0;
        [LabelWidth(200)] public ReputationRank currentReputationRank;
        private int[] ReputationCeils;
        private const int RANK_MAX = 3; // temp
        bool rankingUp, rankingDown;

        public void UpdateReputationRank(int additionalReputation, int[] reputationCeils)
        {
            ReputationCeils = new int[reputationCeils.Length]; 
            Array.Copy(reputationCeils, ReputationCeils, reputationCeils.Length); 

            currentReputationValue += additionalReputation;

            do
            {
                rankingUp = currentReputationValue - reputationCeils[(int)currentReputationRank + 1] >= 0;
                rankingDown = Math.Sign(additionalReputation) < 0;

                int newRankToInt = (int)currentReputationRank + (rankingDown ? -1 : 1);

                // nothing more to do if just changing reputation value but not rank
                if (rankingDown || rankingUp)
                {
                    // rank upgrade
                    currentReputationRank = (ReputationRank)Math.Clamp(Math.Min(newRankToInt, RANK_MAX), 0, RANK_MAX);

                    // carrry over
                    if (rankingUp)
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
            } while (rankingUp || rankingDown); 
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
}
