using Sirenix.OdinInspector;
using StatusUnknown;
using StatusUnknown.Content.Narrative;
using TMPro;
using UnityEngine;

namespace Aurore.DialogSystem
{
    public enum DialogueType { AutoTrigger, Manual }

    public class DialogueComponent : MonoBehaviour
    {
        [Header("Dialogue Data")]
        [SerializeField] protected TMP_Text dialogueTitle;
        [SerializeField] private DialogueType dialogueType;
        [Space, SerializeField] private DialogGraph dialogueGraph;
        [SerializeField] private AudioClip audioMumblingVoice;


        private void OnEnable()
        {
            dialogueTitle.text = dialogueGraph.name.Replace("_", " ");
        }

        /// <summary>
        /// Start the dialog sequence.
        /// </summary>
        public void StartSequence()
        {
            dialogueGraph.Init();
            DialogueUiManager.Instance.Init(dialogueGraph.GetRoot(), audioMumblingVoice);
        }

        // TODO : Edit DialogGraph.GetRoot() if root is conditional (for ex: character has item, has already visited, etc..)
        // Give quest rewards
        public void OnQuestValidated()
        {
            if (dialogueGraph.CurrentDialogueQuestsDone)
            {
                dialogueGraph.GiveReward(); 
            }
            else
            {
                Debug.Log("Some quest was not fullfilled");
            }
        }

        /// <summary>
        /// Getter for the dialogue graph.
        /// </summary>
        public DialogGraph Graph => dialogueGraph;
    }
}
