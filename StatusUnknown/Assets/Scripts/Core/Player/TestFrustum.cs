using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFrustum : MonoBehaviour
{
    // Detects manually if obj is being seen by the main camera

    Collider objCollider;

    Camera cam;
    Plane[] planes;

    void Start()
    {
        cam = Camera.main;
        planes = GeometryUtility.CalculateFrustumPlanes(cam);
        objCollider =  GetComponent<Collider>();
    }

    void Update()
    {
        if (GeometryUtility.TestPlanesAABB(planes, objCollider.bounds))
        {
            Debug.Log(gameObject.name + " has been detected!");
        }
        else
        {
            Debug.Log("Nothing has been detected");
        }
    }
}
