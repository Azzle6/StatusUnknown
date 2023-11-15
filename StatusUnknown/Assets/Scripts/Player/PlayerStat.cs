using UnityEngine.Serialization;

namespace Player
{
    using UnityEngine;
    using Sirenix.OdinInspector;
    
    [CreateAssetMenu(fileName = "PlayerStat", menuName = "CustomAssets/PlayerStat", order = 1)]
    public class PlayerStat : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed;

        [Header("Aim")]
        public float turnAngleLimit = 15f;
        [Range(0.01f, 0.1f)] public float turnSpeed;
        public AnimationCurve angleRequiredMultiplierByDistance;
        public float headHeightOffset = 0.25f;
        public float timeBeforeStopAiming = 2f;
        public LayerMask aimLayerMask;
        [Header("Inertia")]
        public float inertiaDuration;
        public AnimationCurve inertiaCurve;

        
        [Header("Player State ")]
        [ReadOnly] public bool isAiming;
        [ReadOnly] public bool isShooting;
        [ReadOnly] public bool[] weaponMelee = new bool[2];

    }
}
