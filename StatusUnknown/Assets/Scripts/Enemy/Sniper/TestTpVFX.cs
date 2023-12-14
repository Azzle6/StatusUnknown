using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class TestTpVFX : MonoBehaviour
{
    [SerializeField]
    VisualEffect tpVFX;
    [SerializeField] Transform testTransform;
    private void Update()
    {
        tpVFX.SetVector3("TargetPosition", testTransform.position);
    }
}
