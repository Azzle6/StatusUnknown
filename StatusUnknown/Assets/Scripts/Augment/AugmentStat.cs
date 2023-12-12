using Sirenix.OdinInspector;

namespace Augment
{
    using UnityEngine;

    public class AugmentStat : ScriptableObject
    {
        public float augmentCooldown; 
        public Texture2D augmentSprite;
        [ReadOnly] public int augmentSlot;
    }

}
