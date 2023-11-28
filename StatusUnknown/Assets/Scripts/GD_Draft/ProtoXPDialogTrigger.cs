using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ProtoXPDialogTrigger : MonoBehaviour
{
    public ProtoFXDialogSO dialogSO;
    bool activated = true;
    public BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerAction>() != null && activated)
        {
            ProtoXPDialogManager.instance.StartDialog(dialogSO);
            activated = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position + collider.center, collider.size);

        GUI.color = Color.white;
        Handles.Label(transform.position + collider.center, dialogSO.name);
    }
}
