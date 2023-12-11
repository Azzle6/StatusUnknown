using Player;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ProtoXPDialogTrigger : MonoBehaviour
{
    public ProtoFXDialogSO dialogSO;
    bool activated = true;
    public BoxCollider collider;

    public bool activateOnce = false;

    public bool inputTrigger = false;
    public InputAction playerInput;
    public GameObject interactionUI;
    public TextMeshProUGUI dialogName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAction>() != null && activated)
        {
            if(!inputTrigger)
            {
                ProtoXPDialogManager.instance.StartDialog(dialogSO);
                if (activateOnce)
                {
                    activated = false;
                }
            }
            else if (inputTrigger)
            {
                playerInput.Enable();
                SetInteractionFeedback(true);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(inputTrigger && other.GetComponent<PlayerAction>() != null)
        {
            playerInput.performed += context =>
            {
                if (context.performed)
                {
                    ProtoXPDialogManager.instance.StartDialog(dialogSO);
                    playerInput.Disable();
                    if (activateOnce)
                    {
                        activated = false;
                    }
                }
            };
        }
    }

    private void OnTriggerExit(Collider other)
    {
        playerInput.Disable();
        SetInteractionFeedback(false);
    }

    void SetInteractionFeedback(bool state)
    {
        interactionUI.SetActive(state);

        if (dialogSO.displayName)
        {
            DisplayName(state);
            ChangeName(dialogSO.dialogName);
        }
    }

    void DisplayName(bool enable)
    {
        dialogName.gameObject.SetActive(enable);
    }

    void ChangeName(string newText)
    {
        dialogName.text = newText;
    }


    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + collider.center, collider.size);

        GUI.color = Color.white;
        Handles.Label(transform.position + collider.center, dialogSO.name);
    }
    #endif
}
