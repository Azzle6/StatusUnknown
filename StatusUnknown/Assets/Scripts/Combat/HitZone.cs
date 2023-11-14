using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HitZone : MonoBehaviour
{
    enum ZoneType { BOX,SPHERE}
    [SerializeField] ZoneType zoneType = ZoneType.BOX;
    [SerializeField,ShowIf("zoneType", ZoneType.BOX)] Vector3 size = Vector3.one;
    [SerializeField, ShowIf("zoneType", ZoneType.SPHERE)] float  radius = 1f;
    Mesh mesh;
    [SerializeField] LayerMask hitMask;
    [SerializeField] float updateFrequence = 1f;
    Coroutine check;
    HashSet<Collider> temp_colliders = new HashSet<Collider>();
    public event Action<IDamageable> HitTriggerEvent,HitStayEvent; // HitStayEvent call is udpateFrequence based


    private void OnDisable()
    {
        StopCoroutine(check);
    }
    private void OnEnable()
    {
        check = StartCoroutine(checkIDamageable());
        
    }
    IEnumerator checkIDamageable()
    {
        
        yield return new WaitForSeconds(updateFrequence);
        CheckMethod();
        check = StartCoroutine(checkIDamageable());
    }

    void CheckMethod()
    {
        Collider[] colliders;
        if(zoneType == ZoneType.BOX) { }
        colliders = Physics.OverlapBox(transform.position, size * 0.5f, transform.rotation, hitMask); 
        if(zoneType == ZoneType.SPHERE)
            colliders = Physics.OverlapSphere(transform.position,radius,hitMask);

        foreach (Collider collider in colliders)
        {
            IDamageable Idamageable = collider.gameObject.GetComponent<IDamageable>();
            if (Idamageable != null )
            {
                if (!temp_colliders.Contains(collider) && HitTriggerEvent != null)
                {
                    HitTriggerEvent(Idamageable);
                    temp_colliders.Add(collider);
                }
                else if (HitStayEvent != null) { }
                {
                    HitStayEvent(Idamageable);
                }
            }
            
        }
        temp_colliders = colliders.ToHashSet();
       
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = temp_colliders.Count > 0?Color.red : Color.green;
        if (!isActiveAndEnabled)
            Gizmos.color = new Color(0.5f,0.5f,0.5f,0.5f);

        if(zoneType == ZoneType.SPHERE)
            Gizmos.DrawWireSphere(transform.position, radius);

        if(mesh == null)
            mesh =  Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        if(zoneType == ZoneType.BOX)
            Gizmos.DrawWireMesh(mesh,transform.position,transform.rotation,size);
    }
#endif
}
