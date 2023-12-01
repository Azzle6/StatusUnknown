

namespace Weapon
{
    using UnityEngine;
    using System.Collections;
    using Core.Pooler;
    using UnityEngine.VFX;
    
    public class Projectile : MonoBehaviour
    {
        [HideInInspector] public float damage;
        [SerializeField] private Rigidbody rb;
        [SerializeField] private VisualEffectAsset hitVFX;
        [SerializeField] private HitContext hitContext;
        private VisualEffect tempHitVFX;
        public float knockbackStrength = 10f;
        public float lifeTime = 5f;
        
        private void OnEnable()
        {
            StartCoroutine(DestroyAfterTime());
        }
        
        private void OnDisable()
        {
            hitContext.HitTriggerEvent -= Hit;
            StopAllCoroutines();
        }
        
        public void Launch(float damage, Vector3 direction, float speed)
        {
            this.damage = damage;
            rb.velocity = direction * speed;
        }
    
        public void Hit(IDamageable target)
        {
            tempHitVFX = ComponentPooler.Instance.GetPooledObject<VisualEffect>("EmptyVisualEffect");
            tempHitVFX.visualEffectAsset = hitVFX;
            tempHitVFX.transform.position = transform.position;
            tempHitVFX.Play();
            target.TakeDamage(damage, transform.forward * knockbackStrength);

            ComponentPooler.Instance.ReturnObjectToPool(this);
        } 
        
        private IEnumerator DestroyAfterTime()
        {
            yield return new WaitForSeconds(lifeTime);
            ComponentPooler.Instance.ReturnObjectToPool(this);
        }
    }
}


