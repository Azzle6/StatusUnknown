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
        [Header("NPC Data")]
        [SerializeField] private NpcSO npcData; 

        [Header("Dialogue Data")]
        [SerializeField] protected TMP_Text dialogueTitle;
        [SerializeField] private DialogueType dialogueType;

        [Header("Player Data")]
        [SerializeField] private PlayerDataSO playerData;

        private ReputationRank currentPlayerReputationRank;
        private QuestDataSO currentQuestPool;
        private DialogGraph currentDialogueGraph;

        //[SerializeField] private UnityEvent OnReachingFactionMainQuest; 

        /// <summary>
        /// Start the dialog sequence.
        /// </summary>
        public void StartDialogue()
        {
            currentPlayerReputationRank = playerData.GetReputationRank_Simple(npcData.Faction);
            currentQuestPool = npcData.secondaryQuests;

            if (playerData.CanUnlockFactionMainQuest(npcData.Faction))
            {
                currentQuestPool = npcData.mainQuests;
            }

            UpdateCurrentDialogueGraph();

            currentDialogueGraph.Init();
            DialogueUiManager.Instance.Init(currentDialogueGraph.GetRoot(), npcData.npcVoice);
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

            Debug.Log($"player hit new rank with faction {npcData.Faction}"); 
            playerData.UpdatePlayerRank(npcData.Faction);
        }
        #endregion 
    }
}
