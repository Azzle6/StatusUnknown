using UnityEngine;

namespace Core.Player
{
    using Core.SingletonsSO;
    
    [CreateAssetMenu(fileName = "PlayerStat", menuName = "CustomAssets/PlayerStat", order = 1)]
    public class PlayerStat : SingletonSO<PlayerStat>
    {
        [Header("Movement")]
        public float moveSpeed;
        [Header("Inertia")]
        public AnimationCurve inertiaCurve;
        public float inertiaDuration;
        [Range(0.01f, 0.1f)] public float turnSpeed;
    }
}
