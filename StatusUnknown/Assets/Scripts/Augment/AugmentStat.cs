namespace Augment
{
    using UnityEngine;

    public class AugmentStat : ScriptableObject
    {
        public float augmentCooldown; 
        public Texture2D augmentSprite;
        [HideInInspector] public int augmentSlot;
    }

}
