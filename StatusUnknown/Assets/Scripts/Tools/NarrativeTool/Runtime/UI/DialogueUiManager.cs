using System.Collections.Generic;
using TMPro;
using UnityEngine;
using StatusUnknown.Tools.Narrative;
using UnityEngine.UI;
using StatusUnknown.Content.Narrative;
using static StatusUnknown.Content.Narrative.SavedDialogueLinesSO;

namespace Aurore.DialogSystem
{
    public class DialogueUiManager : DialogueManagerMaster
    {
        #region Singleton declaration & Awake

        // ReSharper disable once InconsistentNaming
        public static DialogueUiManager Instance;
        protected override void Awake() 
        {
            base.Awake(); //needed to instantiate the event.
            if (Instance is not null && Instance != this) Destroy(gameObject);
            Instance = this;
            _IsOpenAnimHash = Animator.StringToHash("IsOpen");

            //RefreshSerializedDictionary();
        }

        #endregion

        [SerializeField] private GameObject simpleDialog;
        [SerializeField] private GameObject fullDialogue;
        [SerializeField] private GameObject answer;
        [SerializeField] private Animator animator;
        protected Button[] dialogueAnswers;
        private int _IsOpenAnimHash;

        [Header("Dialogue Save Data")]
        [SerializeField] private SavedDialogueLinesSO playerSavedDialogues;

        // TODO : use this data for gameplay logic
        private static Dictionary<(int, Vector2), DialogueDataHolder> dialogueDatas = new Dictionary<(int, Vector2), DialogueDataHolder>();


        protected override void Start()
        {
            base.Start();
            dialogueAnswers = new Button[answer.transform.childCount];
            for (int i = 0; i < dialogueAnswers.Length; i++)
            {
                dialogueAnswers[i] = answer.transform.GetChild(i).GetComponent<Button>();             
            }

            dialogueDatas.Clear(); 
            dialogueDatas = playerSavedDialogues.GetSavedDialogueData();
        }

        #region Display

        protected sealed override void StartDialogueDisplay()
        {
            animator.SetBool(_IsOpenAnimHash,true);
        }

        protected sealed override void EndDialogueDisplay()
        {
            animator.SetBool(_IsOpenAnimHash,false);
        }

        public sealed override void SetActivationState()
        {
            base.SetActivationState();

            simpleDialog.SetActive(false);
            fullDialogue.SetActive(false);
        }


        protected sealed override void UpdateDialogueSimple(DialogueNode node)
        {
            //Display the correct GameObject.
            simpleDialog.SetActive(true);
            fullDialogue.SetActive(false);
            //Text Modification
            var tmp = simpleDialog.transform.GetChild(0).GetComponent<TMP_Text>();

            StopAllCoroutines(); 
            StartCoroutine(TypeSentence(node.dialogueOpening, tmp));
            //simpleDialog.transform.GetChild(0).GetComponent<TMP_Text>().text = node.content;
        }

        protected sealed override void UpdateDialogueFull(DialogueNode node) { }

        protected override void HideAnswersUI(bool b) => answer.SetActive(!b);

        protected sealed override void UpdateAnswers(List<string> answers)
        {
            HideAnswersUI(false);

            //Display the correct amount of answers and adjust their text content.
            for (var i = 0; i < answer.transform.childCount; i++)
            {
                var child = answer.transform.GetChild(i);
                if (i < answers.Count)
                {
                    child.gameObject.SetActive(true);
                    child.GetComponentInChildren<TMP_Text>().text = answers[i];
                }
                else
                {
                    child.gameObject.SetActive(false);
                }
            }
        }

        public override void OnAnswerClicked(int index)
        {
            // here for aditional dialogueline logic before continuing graph execution
            // like storing a player choice, triggering a gameplay event, etc..
            if (DialogueLines[index].storeIfSelected)
            {
                Debug.Log("this line has been stored");
                //DialogGraph.StoreDialogueLine(currentNode, DialogueLines[index], index); // BUG : serialization not working..
                //Store(DialogueLines[index]);
                StoreDialogueLineInfos(currentNode, DialogueLines[index], index); 
            }

            base.OnAnswerClicked(index);
        }

        private void StoreDialogueLineInfos(DialogueNode current, DialogueLine dialogueLine, int index)
        {
            //if (playerSavedDialogues.allStoredDialogueLines.ContainsKey((current.position, index))) return; 

            /* playerSavedDialogues.allStoredDialogueLines.Add((current.position, index), dialogueLine); 
            Debug.Log("storing dialogue line" + playerSavedDialogues.allStoredDialogueLines); */
            playerSavedDialogues.savedDialoguesData.Add(new SavedDialogueLinesSO.DialogueDataHolder(dialogueLine, current.position, index)); 
        }

        #endregion

        #region Interaction


        public override void OnSkipHovered(bool b)
        {
            if (!hasNoAnswers)
            {
                simpleDialog.transform.GetChild(1).gameObject.SetActive(false);
                return;
            }
            simpleDialog.transform.GetChild(1).gameObject.SetActive(b);
        }

        #endregion
    }
}
