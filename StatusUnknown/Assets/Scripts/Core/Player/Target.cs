using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] public float radius;
    [SerializeField] private Transform player;

    private void OnEnable()
    {
        radius = sphereCollider.radius;
    }
}
