using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ProtoChess : MonoBehaviour
{
    public InputAction interactInput;
    public GameObject textInteract;
    public GameObject textLoot;
    public bool lootable= true;
    
    void Start()
    {
        interactInput.Disable();
    }
    
    void Update()
    {
        if (interactInput.IsPressed() && lootable)
        {
            textInteract.SetActive(false);
            textLoot.SetActive(true);
            lootable = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player" && lootable)
        {
            interactInput.Enable();
            textInteract.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            interactInput.Disable();
            textInteract.SetActive(false);
            textLoot.SetActive(false);
        }
    }
}
