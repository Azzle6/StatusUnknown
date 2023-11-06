using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandPosition : MonoBehaviour
{
    [SerializeField] private Transform handTransform;

    private void LateUpdate()
    {
        transform.position = handTransform.position;
        transform.rotation = Quaternion.Euler(handTransform.rotation.eulerAngles);
    }
}
