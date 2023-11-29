using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UICamOrienter : MonoBehaviour
{
    Transform camT;
    private void Start()
    {
        camT = Camera.main.transform;
    }

    private void Update()
    {
        if(camT != null)
            transform.forward = -camT.forward;
    }
}
