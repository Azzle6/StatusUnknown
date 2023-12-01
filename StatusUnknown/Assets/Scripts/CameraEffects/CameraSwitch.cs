using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Core.Player;

public class CameraSwitch : MonoBehaviour
{
    public GameObject[] activateCams;
    public GameObject[] disactivateCams;

    /*private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerAction>() != null)
        {
            for(int i = 0; i < disactivateCams.Length; i++)
            {
                disactivateCams[i].SetActive(false);
            }
            for (int i = 0; i < activateCams.Length; i++)
            {
                activateCams[i].SetActive(true);
            }
        }
    }*/
}
