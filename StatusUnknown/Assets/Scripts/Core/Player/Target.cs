using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float radius;
    [SerializeField] private Transform player;
}
