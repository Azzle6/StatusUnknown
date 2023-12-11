using Core.Pooler;

namespace Weapon
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.VFX;

    
    public class ReturnToPoolEmptyVFX : MonoBehaviour
    {
        [SerializeField] private float lifeTime = 2f;
        [SerializeField] private VisualEffect vfx;
        private void OnEnable()
        {
            StartCoroutine(DestroyAfterTime());
        }
        
        private void OnDisable()
        {
            StopAllCoroutines();
        }
        
        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(lifeTime);
            ComponentPooler.Instance.ReturnObjectToPool(vfx);
        }
    }

}