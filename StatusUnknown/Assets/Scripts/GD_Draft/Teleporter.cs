using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Teleporter : MonoBehaviour
{
    public GameObject goal;
    public bool onCD = false;
    public InputAction interactInput;
    private GameObject player;

    private void Start()
    {
        interactInput.Disable();
    }

    private void Update()
    {
        if (interactInput.IsPressed() && onCD == false)
        {
            onCD = true;
            goal.GetComponent<Teleporter>().onCD = true;
            player.GetComponent<Transform>().position = goal.transform.position;
            StartCoroutine("Cooldown");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interactInput.Enable();
        if (other.name == "Player" && onCD==false)
        {
            player = other.GameObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactInput.Disable();
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(3);
        onCD = false;
        goal.GetComponent<Teleporter>().onCD = false;
    }
}