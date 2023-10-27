namespace Core.Player
{
    using UnityEngine;
    using Core.SingletonsSO;
    
    [CreateAssetMenu(fileName = "PlayerStat", menuName = "CustomAssets/PlayerStat", order = 1)]
    public class PlayerStat : ScriptableObject
    {
        [Header("Movement")]
        public float moveSpeed;
        [Header("Inertia")]
        public float inertiaDuration;
        public AnimationCurve inertiaCurve;
        [Range(0.01f, 0.1f)] public float turnSpeed;
        [Header("Aim")]
        public AnimationCurve angleRequiredMultiplierByDistance;
        [HideInInspector] public bool isAiming;

    }
}
