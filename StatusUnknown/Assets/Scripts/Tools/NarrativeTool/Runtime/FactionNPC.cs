using TMPro;
using UnityEngine;
using UnityEngine.Events;
using StatusUnknown.Content;
using System;
using StatusUnknown.Content.Narrative;

namespace Aurore.DialogSystem
{
    public enum DialogueType { AutoTrigger, Manual }

    public class FactionNPC : MonoBehaviour
    {
        // stored into a dictionary with key = currentRank
        // get random quest pool
        [Serializable]
        public struct NPCSecondaryQuests
        {
            public ReputationRank rank;
            public DialogGraph secondaryQuestDialogue;
            public QuestSO[] secondaryQuestsPool;
        }

        // stored into a dictionary with key = currentRank
        [Serializable]
        public struct NPCMainQuests
        {
            public ReputationRank rank;
            public DialogGraph mainQuestDialogue;
        }

        [Header("Player Data")] // TEMP
        [SerializeField] private PlayerDataSO playerData; 

        [Header("Dialogue Data")]
        [SerializeField] protected TMP_Text dialogueTitle;
        [SerializeField] private DialogueType dialogueType;
        [Space, SerializeField] private NPCMainQuests[] mainQuests;
        [SerializeField] private NPCSecondaryQuests[] secondaryQuests;
        [SerializeField] private AudioClip audioMumblingVoice;
        private DialogGraph currentDialogueGraph;

        [SerializeField] private UnityEvent OnReachingFactionMainQuest; 

        private void OnEnable()
        {
            Init(); 
        }

        private void Init()
        {
            currentDialogueGraph = secondaryQuests[0].secondaryQuestDialogue;

            (bool canUnlockNextFactionMainQuest, int nextRankIndex) = currentDialogueGraph.CanUnlockFactionMainQuest(); 
            if (canUnlockNextFactionMainQuest)
            {
                OnReachingFactionMainQuest.Invoke();
                UpdateCurrentDialogueGraph(nextRankIndex); 
            }

            dialogueTitle.text = mainQuests[0].mainQuestDialogue.name.Replace("_", " ");
        }

        private void UpdateCurrentDialogueGraph(int nextRankIndex)
        {
            currentDialogueGraph = mainQuests[nextRankIndex].mainQuestDialogue;
        }

        /// <summary>
        /// Start the dialog sequence.
        /// </summary>
        public void StartSequence()
        {
            currentDialogueGraph.Init();
            DialogueUiManager.Instance.Init(currentDialogueGraph.GetRoot(), audioMumblingVoice);
        }

        // TODO : Edit DialogGraph.GetRoot() if root is conditional (for ex: character has item, has already visited, etc..)
        // Give quest rewards
        public void OnQuestValidated()
        {
            if (currentDialogueGraph.CurrentDialogueQuestsDone)
            {
                currentDialogueGraph.GiveReward(); 
            }
            else
            {
                Debug.Log("Some quest was not fullfilled");
            }
        }
    }
}
