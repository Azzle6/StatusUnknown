using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDoor : MonoBehaviour
{
    bool isOpen;
    Animator animator;


    private void OnTriggerEnter(Collider other)
    {
        
    }

    private void OnTriggerExit(Collider other)
    {
        
    }

    void ChangeDoorState(bool open = true)
    {
        animator.SetTrigger("trigger");
    }
}
