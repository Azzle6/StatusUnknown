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

        public void UpdateReputationRank(int additionalReputation, int[] reputationCeils)
        {
            ReputationCeils = new int[reputationCeils.Length]; 

            Array.Copy(reputationCeils, ReputationCeils, reputationCeils.Length); 
            Debug.Log($"reputation update from {currentReputationValue} to {currentReputationValue + additionalReputation}");
            currentReputationValue += additionalReputation;

            do
            {
                Debug.Log("New Rank was reached ! ");
                currentReputationRank = (ReputationRank)Mathf.Min((int)currentReputationRank + 1, 3);
                currentReputationValue -= ReputationCeils[(int)currentReputationRank];
            }
            while (currentReputationValue >= ReputationCeils[(int)currentReputationRank + 1]);
            // remainder is kept
            Debug.Log("remainder xp : " + currentReputationValue); 
        }
    }

    [DisableInEditorMode, DisableInPlayMode] public Dictionary<Faction, RankData> rankDatas = new Dictionary<Faction, RankData>();

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
}
