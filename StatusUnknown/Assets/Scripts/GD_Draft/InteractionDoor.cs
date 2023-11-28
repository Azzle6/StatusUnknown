using Player;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InteractionDoor : MonoBehaviour
{
    public InputAction playerInput;
    public Collider triggerZone;
    public GameObject interactionUI;
    public Slider holdSlider;
    public Animator animator;

    float holdValue = 0;
    public float holdDuration = 1;
    public float holdDecrement = 0.05f;

    private void Awake()
    {
        SetInteractionFeedback(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        playerInput.Enable();
        SetInteractionFeedback(true);
    }

    private void OnTriggerStay(Collider other)
    {
        bool interactionInput = playerInput.IsPressed();

        if (other.GetComponent<PlayerAction>() != null && interactionInput)
        {            
            holdValue += Time.deltaTime;
            if (holdValue >= holdDuration)
            {
                OpenDoor();
            }
        }
        else if (other.GetComponent<PlayerAction>() != null && holdValue > 0)
        {
            holdValue -= holdDecrement;
        }

        holdSlider.value = holdValue;
    }

    private void OnTriggerExit(Collider other)
    {
        playerInput.Disable();
        SetInteractionFeedback(false);

        holdValue = 0;
    }

    void SetInteractionFeedback(bool state)
    {
        interactionUI.SetActive(state);
    }

    void OpenDoor()
    {
        triggerZone.enabled = false;
        animator.SetBool("IsOpen", true);
        SetInteractionFeedback(false);
    }
}
