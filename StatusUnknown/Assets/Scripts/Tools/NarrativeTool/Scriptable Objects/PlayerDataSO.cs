using Sirenix.OdinInspector;
using StatusUnknown;
using StatusUnknown.Content.Narrative;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Status Unknown/Gameplay/Player Data")]
public class PlayerDataSO : SerializedScriptableObject
{
    public enum ReputationRank { Zero, One, Two, Three }
    [field: SerializeField] public QuestJournalSO QuestJournal { get; private set; }

    private Action<int, Faction, QuestObjectSO> OnQuestCompletion;
    [field: SerializeField] private List<QuestObjectSO> QuestRewards { get; set; }

    public class RankData
    {
        [LabelWidth(200)] public int currentReputationValue = 0;
        [LabelWidth(200)] public ReputationRank currentReputationRank;
        private int[] ReputationCeils;
        private const int RANK_MAX = 3; // temp

        public void UpdateReputationRank(int additionalReputation, int[] reputationCeils)
        {
            ReputationCeils = new int[reputationCeils.Length]; 
            Array.Copy(reputationCeils, ReputationCeils, reputationCeils.Length); 

            currentReputationValue += additionalReputation;

            do
            {
                int modifier = MathF.Sign(additionalReputation); 
                currentReputationRank = (ReputationRank)Mathf.Clamp(Mathf.Min((int)currentReputationRank + modifier, RANK_MAX), 0, RANK_MAX);

                if (modifier > 0)
                {
                    currentReputationValue -= ReputationCeils[(int)currentReputationRank];
                }
                else
                {
                    currentReputationValue = currentReputationRank == ReputationRank.Zero ?
                        0 :
                        ReputationCeils[(int)currentReputationRank + 1] - currentReputationValue;
                }

                Debug.Log($"New rank : {currentReputationRank} \n With remainder xp : {currentReputationValue}");
            }
            while (currentReputationValue >= ReputationCeils[Mathf.Clamp((int)currentReputationRank + 1, 0, RANK_MAX)]);
            // remainder is kept

            if (currentReputationRank == (ReputationRank)RANK_MAX)
            {
                Debug.Log("max rank was hit");
                currentReputationValue = 0;
                return;
            }
        }
    }

    public Dictionary<Faction, RankData> rankDatas = new Dictionary<Faction, RankData>();

    // système de progression joueur ?
    // grinder pour débloquer la quête main
    // main quest complete -> rank supérieur 

    // TODO : move this to Faction Editor
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

    public void UpdateCurrentReputation(int additionalReputation, Faction key, QuestObjectSO questReward = null)
    {
        rankDatas[key].UpdateReputationRank(additionalReputation, reputationCeils[(int)key]);

        if (questReward != null)
        {
            QuestRewards.Add(questReward);
        }
    }

    public (int, ReputationRank) GetCurrentReputation(Faction key)
    {
        RankData rd = rankDatas[key];
        (int xpRemainder, ReputationRank reputationRank) returnValue = new(rd.currentReputationValue, rd.currentReputationRank);

        return returnValue; 
    }
}
