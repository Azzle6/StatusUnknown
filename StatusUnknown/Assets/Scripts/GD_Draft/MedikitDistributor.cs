using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MedikitDistributor : MonoBehaviour
{
    public GameObject medikit;
    public float cD;
    public bool onCD = false;
    public InputAction interactInput;
    public GameObject canvas;
    public Transform spawnPoint;

    private void Start()
    {
        interactInput.Disable();
    }

    private void Update()
    {
        if (interactInput.IsPressed() && onCD == false)
        {
            Instantiate(medikit, spawnPoint);
            StartCoroutine("Cooldown");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        interactInput.Enable();
        canvas.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        interactInput.Disable();
        canvas.SetActive(false);
    }

    IEnumerator Cooldown()
    {
        onCD = true;
        yield return new WaitForSeconds(cD);
        onCD = false;
    }
}