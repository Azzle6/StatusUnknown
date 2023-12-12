using Core.SingletonsSO;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace UI
{
    using UnityEngine;

    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement dialogueContainer;
        private Label timerLabel;
        private Label dialogueLabel;
        private Button closeButton;
        private VisualElement imageDisplayer;
        
        void Start()
        {
            timerLabel = uiDocument.rootVisualElement.Q<Label>("Timer");
            dialogueContainer = uiDocument.rootVisualElement.Q<VisualElement>("Dialogue");
            closeButton = uiDocument.rootVisualElement.Q<Button>("CloseButton");
            imageDisplayer = uiDocument.rootVisualElement.Q<VisualElement>("ImageDisplayer");

            Debug.Log(SceneManager.GetActiveScene().name);
            if (SceneManager.GetActiveScene().name == "ProtoXP")
            {
                timerLabel.style.display = DisplayStyle.Flex;

            }
            closeButton.clicked += CloseDialogue;
            dialogueLabel = uiDocument.rootVisualElement.Q<Label>("DialogueLabel");
            CloseDialogue();
            
        }
        
        private void OpenDialogue(ProtoFXDialogSO dialogSo)
        {
            if (dialogSo.displayImage)
            {
                imageDisplayer.style.display = DisplayStyle.Flex;
                imageDisplayer.style.backgroundImage = dialogSo.image.texture;
            }
            else
            {
                imageDisplayer.style.display = DisplayStyle.None;
            }
            {
                dialogueContainer.style.display = DisplayStyle.Flex;
                dialogueLabel.text = dialogSo.text;
                UIHandler.Instance.ForceFocus(closeButton);
            }
 
        }
        
        private void CloseDialogue()
        {
            dialogueContainer.style.display = DisplayStyle.None;
            imageDisplayer.style.display = DisplayStyle.None;
        }

        public void OnDialogueEvent(ProtoFXDialogSO dialogue)
        {
            if (dialogue != null)
            {
                if (dialogue.justTimer)
                    return;
                
                OpenDialogue(dialogue);
            }
            else
            {
                CloseDialogue();
            }      
        }
    }

}