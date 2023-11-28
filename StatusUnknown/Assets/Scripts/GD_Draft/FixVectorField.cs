using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VectorField;

public class FixVectorField : MonoBehaviour
{
    public VectorFieldVolume vectorField;

    private void Start()
    {
        vectorField.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        vectorField.enabled = true;
    }

    private void OnTriggerExit(Collider other)
    {
        vectorField.enabled = false;
    }
}
