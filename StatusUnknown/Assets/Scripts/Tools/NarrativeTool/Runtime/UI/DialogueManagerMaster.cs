using StatusUnknown.Content.Narrative;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using static XNode.Node;

namespace Aurore.DialogSystem
{
    [RequireComponent(typeof(AudioSource))]
    public abstract class DialogueManagerMaster : MonoBehaviour
    {
        [Header("General")]
        [Space, SerializeField] protected GameObject startButtonObj;
        [SerializeField] protected GameObject endButtonObj;

        [Header("Quest Handling")]
        [Space, SerializeField] private QuestJournalSO questJournal;
        [SerializeField, Output] private QuestSO givenQuest;

        private bool questFieldIsNull = true;
        private bool questRemoved = false;

        protected DialogueNode currentNode;
        protected bool canBeSkipped = false;

        [Header("Events")]
        [Space, SerializeField] protected UnityEvent startDialogueEvent;
        [SerializeField] protected UnityEvent endDialogueEvent;
        [SerializeField] protected UnityEvent questValidationEvent;

        private AudioSource _source;
        private event Action<DialogueNode> OnStartEnded;

        private event Action<List<string>> OnSentenceTypingDone;
        private event Action OnFinalSentenceTypingDone; 

        public void UpdateOnStartEnded() => OnStartEnded?.Invoke(currentNode);

        protected virtual void Awake()
        {
            OnStartEnded += UpdateDialogue;
            OnFinalSentenceTypingDone += End;
            OnSentenceTypingDone += UpdateAnswers; 
            _source = GetComponent<AudioSource>();
            _waitLetter = new WaitForSeconds(waitTimeLetter);
        }

        private void Start()
        {
            SetActivationState(); 
        }

        /// <summary>
        /// This method is called whenever a Component needs to start a sequence.
        /// </summary>
        /// <param name="root">The root node of a DialogGraph</param>
        /// <param name="voice"></param>
        /// <exception cref="NullReferenceException">Thrown if the given node is null</exception>
        public void Init(DialogueNode root, AudioClip voice = null)
        {
            // ReSharper disable once JoinNullCheckWithUsage
            if (root is null) throw new NullReferenceException("Root element of a graph must not be null !");
            if (root.GetInputPort("input").IsConnected) throw new MonoRootDialogGraphException($"The provided node in {currentNode.graph.name} is not a Root !!!");

            if (voice) _source.clip = voice;

            startDialogueEvent?.Invoke();
            currentNode = root;
            StartDialogueDisplay();
            UpdateOnStartEnded();
        }

        public virtual void SetActivationState()
        {
            startButtonObj.SetActive(true);
            endButtonObj.SetActive(false);
        }

        private void UpdatePlayerQuestJournal()
        {
            Debug.Log("updating quest journal");

            if (!questFieldIsNull)
            {
                Debug.Log("adding quest");
                questRemoved = false;
                questJournal.AddQuest(givenQuest);
            }
            else
            {
                if (!questRemoved)
                {
                    Debug.Log("removing quest");
                    questRemoved = true;
                    questJournal.RemoveQuest(givenQuest);
                }
            }

            questFieldIsNull = givenQuest == null;
        }

        #region Start & End Logic

        /// <summary>
        /// This is called at the ed of start Dialogue to add your own logic. Don`t forget to start the first node after your logic.
        /// </summary>
        protected abstract void StartDialogueDisplay();

        /// <summary>
        /// This method is called whenever a complete dialogue ends (not one line).
        /// This is seperate to make sure the end event is triggered even with user override.
        /// </summary>
        private void End()
        {
            EndDialogueDisplay();
            endDialogueEvent?.Invoke();
            questValidationEvent?.Invoke();
        }

        /// <summary>
        /// This is called when the all dialogue is ended and can be modified by inheritance
        /// </summary>
        protected abstract void EndDialogueDisplay();

        #endregion

        /// <summary>
        /// This method is in charge of updating the logic when we switch node in a dialog.
        /// This is the core method where the action unfolds.
        /// </summary>
        /// <param name="node">The new node to look at.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the type of the node is unknown.</exception>
        private void UpdateDialogue(DialogueNode node)
        {
            //Update the current Node and deal with the end of a sequence
            currentNode = node;
            if (currentNode is null)
            {
                End();
                return;
            }

            //Displaying content sequence
            switch (currentNode.GetDialogueType())
            {
                case DialogueType.Simple:
                    UpdateDialogueSimple(currentNode);
                    break;
                case DialogueType.Full:
                    UpdateDialogueFull(currentNode);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            //Deal with answers
            canBeSkipped = !currentNode.HasAnswers();
            DealWithAnswers();
        }

        /// <summary>
        /// This method check if we need to print answer or not. If we need it, it calls the corresponding method.
        /// </summary>
        /// <param name="node">The current node</param>
        protected virtual void DealWithAnswers()
        {
            if(canBeSkipped)
            {
                HideAnswersUI(true);
                OnFinalSentenceTypingDone(); 
                return;
            }
            
            UpdateAnswers(currentNode.answers);
        }

        #region UI Tighly linked abstract method

        /// <summary>
        /// Update the UI to display a "simple" dialogue, i.e. text-only dialogue.
        /// </summary>
        /// <param name="node">The node containing the data.</param>
        protected abstract void UpdateDialogueSimple(DialogueNode node);
        
        /// <summary>
        /// Update the UI to display a "full" dialogue, i.e. text+image+title dialogue.
        /// </summary>
        /// <param name="node">The node containing the data.</param>
        protected abstract void UpdateDialogueFull(DialogueNode node);
        /// <summary>
        /// This method hide or show the answers from the UI depending on the given boolean.
        /// </summary>
        /// <param name="b">true if you want to hide the UI.</param>
        protected abstract void HideAnswersUI(bool b);

        /// <summary>
        /// This method update the amount and the content of answers to be displayed.
        /// </summary>
        /// <param name="answers">a string array of the answers to show</param>
        protected abstract void UpdateAnswers(List<string> answers);

        #endregion

        [Range(0.01f,0.1f)]
        [Tooltip("The time in seconds to wait between each letter display in a dialogue.")]
        [SerializeField] private float waitTimeLetter = 0.02f;
        private WaitForSeconds _waitLetter;
        
        /// <summary>
        /// Call this function to animate the letter in when displaying new text.
        /// Pay attention to situation when the user skip an unfinished animation.
        /// </summary>
        /// <param name="newSentence">The new sentence to display</param>
        /// <param name="uiText">The UI Text to write on (TMP_Text only)</param>
        /// <returns></returns>
        protected IEnumerator TypeSentence(string newSentence, TMP_Text uiText)
        {
            uiText.text = ""; //Reset it
            _source.Play();
            foreach (var c in newSentence.ToCharArray())
            {
                uiText.text += c; //Pas ultra opti mais je pense que j'ai pas le choix (pas possible de StringBuilder ici)
                //_source.Play();
                yield return _waitLetter;
            }
            _source.Stop();
        }
        
        #region Interactions
        
        /// <summary>
        /// This method must be called whenever an answer is chosen. Should be linked to your button callback method.
        /// </summary>d
        /// <param name="index">The index of the answers has given in the original answers array.</param>
        public void OnAnswerClicked(int index)
        {
            var node = DialogGraph.GetNext(currentNode, index);
            UpdateDialogue(node);
        }

        /// <summary>
        /// This method implement the behavior of the UI when the user hover the dialog box of the dialogue.
        /// </summary>
        /// <param name="b">true if hovered</param>
        public abstract void OnSkipHovered(bool b);
        /*{
            if (!canBeSkipped)
            {
                simpleDialog.transform.GetChild(1).gameObject.SetActive(false);
                return;
            }
            simpleDialog.transform.GetChild(1).gameObject.SetActive(b);
        }*/
        
        /// <summary>
        /// This method is called to skip the current dialogue and get to the next one in the graph.
        /// WARNING : This method does nothing on a multi-choice dialogue line.
        /// </summary>
        public void OnSkipDialog()
        {
            if (!canBeSkipped) return;

            UpdateDialogue(DialogGraph.GetNext(currentNode, 0));
        }

        #endregion
        
    }
}