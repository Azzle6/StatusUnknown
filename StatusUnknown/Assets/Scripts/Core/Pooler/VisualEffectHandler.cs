using Core.Pooler;
using UnityEngine.VFX;

namespace pooler
{
    using System.Collections;
    using UnityEngine;
    
    
    public class VisualEffectHandler : MonoBehaviour
    {
        [SerializeField] public VisualEffect vfx;

        public void StartVFX(VisualEffectAsset vFXToPlay,float timeBeforeReturningToPool)
        {
            vfx.visualEffectAsset = vFXToPlay;
            vfx.Play();
            StartCoroutine(ReturnToPool(timeBeforeReturningToPool));
        }
        
        public VisualEffect GetVFX()
        {
            return vfx;
        }
        
        private IEnumerator ReturnToPool(float timeBeforeReturningToPool)
        {
            yield return new WaitForSeconds(timeBeforeReturningToPool);
            ComponentPooler.Instance.ReturnObjectToPool(this);
        }
    }
}


