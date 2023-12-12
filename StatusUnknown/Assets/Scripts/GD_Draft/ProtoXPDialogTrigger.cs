using Core.EventsSO.GameEventsTypes;
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
    
    public TextMeshProUGUI dialogName;
    [SerializeField] private DialogueSOGameEvent dialogueSOGameEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAction>() != null && activated)
        {
            Debug.Log("enter");
            ProtoXPDialogManager.instance.StartDialog(dialogSO);
            if (activateOnce)
            {
                activated = false;
                dialogueSOGameEvent.RaiseEvent(dialogSO);

            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("exit");
        if (other.GetComponent<PlayerAction>() != null)
        {
            dialogueSOGameEvent.RaiseEvent(null);
            
        }
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
