using Core.Pooler;
using UnityEngine.VFX;

namespace pooler
{
    using System.Collections;
    using UnityEngine;
    
    
    public class VisualEffectHandler : MonoBehaviour
    {
        [SerializeField] public VisualEffect vfx;

        private Coroutine currentTimer;

        public void StartVFX(VisualEffectAsset vFXToPlay,float timeBeforeReturningToPool)
        {
            vfx.visualEffectAsset = vFXToPlay;
            vfx.Play();
            currentTimer = StartCoroutine(ReturnToPool(timeBeforeReturningToPool));
        }

        public void ForceStop()
        {
            if(this.currentTimer != null)
                StopCoroutine(this.currentTimer);
            this.StopVFX();
        }
        
        private IEnumerator ReturnToPool(float timeBeforeReturningToPool)
        {
            yield return new WaitForSeconds(timeBeforeReturningToPool);
            this.StopVFX();
        }

        private void StopVFX()
        {
            this.currentTimer = null;
            this.vfx.Stop();
            ComponentPooler.Instance.ReturnObjectToPool(this);
        }
    }
}


