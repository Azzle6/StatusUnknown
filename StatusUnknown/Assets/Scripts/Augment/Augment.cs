namespace Augment
{
    using Core.EventsSO;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public abstract class Augment : MonoBehaviour
    {
        public AugmentDataGameEvent augmentDataGameEvent;
        public bool isReady = true;
        public float augmentCooldown;
        
        public AugmentManager augmentManager;

        public virtual void ActionPressed()
        {
            augmentDataGameEvent.RaiseEvent(GetAugmentStat());
        }
        
        public abstract AugmentStat GetAugmentStat(); 
        
        public abstract IEnumerator AugmentCooldownCoroutine();
    }

}

