namespace Augment
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public abstract class Augment : MonoBehaviour
    {
        public Sprite augmentSprite;
        public bool isReady = true;
        public float augmentCooldown;
        
        public AugmentManager augmentManager;
        public abstract void ActionPressed();
        
        
        public abstract IEnumerator AugmentCooldownCoroutine();
    }

}

