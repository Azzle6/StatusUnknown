using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    bool isOpen = false;
    public Animator animator;


    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerAction>() != null)
        {
            ChangeDoorState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<PlayerAction>() != null)
        {
            ChangeDoorState();
        }
    }

    [ContextMenu("TriggerDoor")]
    void ChangeDoorState()
    {
        isOpen = !isOpen;
        animator.SetBool("IsOpen",isOpen);
    }
}
