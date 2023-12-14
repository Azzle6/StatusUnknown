using TMPro;
using UnityEngine;
using StatusUnknown.Content;
using StatusUnknown.Content.Narrative;
using StatusUnknown;

namespace Aurore.DialogSystem
{
    public enum DialogueType { AutoTrigger, Manual }

    public class FactionNPC : MonoBehaviour
    {
        [Header("Player Data")] // TEMP
        [SerializeField] private PlayerDataSO playerData; 

        [Header("Dialogue Data")]
        [SerializeField] protected TMP_Text dialogueTitle;
        [SerializeField] private DialogueType dialogueType;
        [SerializeField] private Faction npcFaction;

        [Space, SerializeField] private MainQuestsDataSO npcMainQuestPools;
        [SerializeField] private SecondaryQuestDataSO npcSecondaryQuestPools;
        [HideInInspector, SerializeField] private AudioClip audioMumblingVoice;

        private ReputationRank currentPlayerReputationRank;
        private QuestDataSO currentQuestPool;
        private DialogGraph currentDialogueGraph;

        //[SerializeField] private UnityEvent OnReachingFactionMainQuest; 

        /// <summary>
        /// Start the dialog sequence.
        /// </summary>
        public void StartDialogue()
        {
            currentPlayerReputationRank = playerData.GetReputationRank_Simple(npcFaction);
            currentQuestPool = npcSecondaryQuestPools;

            if (playerData.CanUnlockFactionMainQuest(npcFaction))
            {
                currentQuestPool = npcMainQuestPools;
            }

            UpdateCurrentDialogueGraph();

            currentDialogueGraph.Init();
            DialogueUiManager.Instance.Init(currentDialogueGraph.GetRoot(), audioMumblingVoice);
        }

        private void UpdateCurrentDialogueGraph()
        {
            currentDialogueGraph = currentQuestPool.GetCurrentDialogue(currentPlayerReputationRank);
            dialogueTitle.text = currentDialogueGraph.name.Replace("_", " ");
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

        #region DEBUG
        public void ValidateMainQuest()
        {
            // only do this if the ongoing quest is a main one
            if (currentQuestPool.GetType() == typeof(SecondaryQuestDataSO)) return;

            Debug.Log($"player hit new rank with faction {npcFaction}"); 
            playerData.UpdatePlayerRank(npcFaction);
        }
        #endregion 
    }
}
