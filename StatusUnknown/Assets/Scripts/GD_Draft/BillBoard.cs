using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillBoard : MonoBehaviour
{
    Camera mainCamera;
    void Start()
    {
        mainCamera = Camera.main;
    }
    void LateUpdate()
    {
        transform.LookAt(mainCamera.transform);
        transform.Rotate(0, 180, 0);
    }
}
