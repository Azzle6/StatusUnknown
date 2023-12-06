using Core.Pooler;
using UnityEngine.VFX;

namespace pooler
{
    using System.Collections;
    using UnityEngine;
    
    
    public class VisualEffectHandler : MonoBehaviour
    {
        [SerializeField] private VisualEffect vfx;

        public void StartVFX(VisualEffectAsset vFXToPlay,float timeBeforeReturningToPool)
        {
            vfx.visualEffectAsset = vFXToPlay;
            vfx.Play();
            StartCoroutine(ReturnToPool(timeBeforeReturningToPool));
        }
        
        private IEnumerator ReturnToPool(float timeBeforeReturningToPool)
        {
            yield return new WaitForSeconds(timeBeforeReturningToPool);
            ComponentPooler.Instance.ReturnObjectToPool(this);
        }
    }
}


