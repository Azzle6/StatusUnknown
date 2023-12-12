using Core.SingletonsSO;
using UnityEngine.UIElements;

namespace UI
{
    using UnityEngine;

    public class DialogueHandler : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        private VisualElement dialogueContainer;
        private Label dialogueLabel;
        private Button closeButton;
        
        void Start()
        {
            dialogueContainer = uiDocument.rootVisualElement.Q<VisualElement>("Dialogue");
            closeButton = uiDocument.rootVisualElement.Q<Button>("CloseButton");
            closeButton.RegisterCallback<ClickEvent>(ev => CloseDialogue());
            dialogueLabel = uiDocument.rootVisualElement.Q<Label>("DialogueLabel");
            CloseDialogue();
            
        }
        
        private void OpenDialogue(ProtoFXDialogSO dialogSo)
        {
            dialogueContainer.style.display = DisplayStyle.Flex;
            dialogueLabel.text = dialogSo.text;
            UIHandler.Instance.ForceFocus(closeButton);
        }
        
        private void CloseDialogue()
        {
            dialogueContainer.style.display = DisplayStyle.None;
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