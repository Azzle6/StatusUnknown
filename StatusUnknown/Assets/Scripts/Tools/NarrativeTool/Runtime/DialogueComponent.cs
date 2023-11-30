using TMPro;
using UnityEngine;

namespace Aurore.DialogSystem
{
    public class DialogueComponent : MonoBehaviour
    {
        [SerializeField] protected TMP_Text dialogueTitle;

        [Space, SerializeField] private DialogGraph dialogueGraph;
        [SerializeField] private AudioClip audioMumblingVoice;

        private void OnEnable()
        {
            dialogueTitle.text = dialogueGraph.name.Replace("_", " "); 
        }

        /// <summary>
        /// Getter for the dialogue graph.
        /// </summary>
        public DialogGraph Graph => dialogueGraph;

        /// <summary>
        /// Start the dialog sequence.
        /// </summary>
        public void StartSequence()
        {
            dialogueGraph.Init(); 
            DialogueUiManager.Instance.Init(dialogueGraph.GetRoot(), audioMumblingVoice);
        }

        // TODO : Edit DialogGraph.GetRoot() if root is conditional (for ex: character has item, has already visited, etc..)

        public void OnQuestValidated()
        {
            if (dialogueGraph.CurrentDialogueQuestsDone)
            {
                Debug.Log("reward added to your inventory : " +  dialogueGraph.GetQuestReward(0));
            }
            else
            {
                Debug.Log("Some quest was not fullfilled"); 
            }
        }
    }
}
