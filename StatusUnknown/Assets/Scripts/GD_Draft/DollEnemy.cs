using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DollEnemy : MonoBehaviour
{
    public InputAction interactInput;
    GameObject player;
    public GameObject doll;
    
    void Start()
    {
        interactInput.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player");
        transform.LookAt(player.transform);
        if (interactInput.IsPressed())
        {
            Destroy(doll);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interactInput.Enable();
    }

    private void OnTriggerExit(Collider other)
    {
        interactInput.Disable();
    }
}
