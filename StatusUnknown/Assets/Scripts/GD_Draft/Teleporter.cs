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
    public GameObject canvas;

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
        canvas.SetActive(true);
        if (other.name == "Player")
        {
            player = other.GameObject();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        interactInput.Disable();
        canvas.SetActive(false);
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(3);
        onCD = false;
        goal.GetComponent<Teleporter>().onCD = false;
    }
}