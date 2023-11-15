using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class HitContext : MonoBehaviour
{
    public LayerMask hitMask;
    [SerializeField,SerializeReference] HitShape hitShape = new HitBox();
    [SerializeField] float updateFrequence = 1f;
    Coroutine processDetection;
    HashSet<Collider> temp_colliders = new HashSet<Collider>();
    public event Action<IDamageable> HitTriggerEvent,HitStayEvent; // HitStayEvent call is udpateFrequence based


    private void OnDisable()
    {
        StopCoroutine(processDetection);
    }
    private void OnEnable()
    {
        processDetection = StartCoroutine(ProcessDetection());
        
    }
    IEnumerator ProcessDetection()
    {
        yield return new WaitForSeconds(updateFrequence);
        ProcessHitEvents();
        processDetection = StartCoroutine(ProcessDetection());
    }

    void ProcessHitEvents()
    {
        var colliders = hitShape.DetectColliders(this);
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

        hitShape.DrawGizmos(this);
    }
#endif
}
