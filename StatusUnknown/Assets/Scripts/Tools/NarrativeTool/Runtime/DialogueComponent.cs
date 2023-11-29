using System;
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
        public void StartSequence() => DialogueUiManager.Instance.Init(dialogueGraph.GetRoot(), audioMumblingVoice);

        // TODO : Edit DialogGraph.GetRoot() if root is conditional (for ex: character has item, has already visited, etc..)
    }
}
