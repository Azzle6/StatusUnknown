using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "Ability_Delayed_Name", menuName = "Status Unknown/Gameplay/Combat/Ability Delayed")]
    public class AbilityConfigSO_Delayed : AbilityConfigSO_Base
    {
        [SerializeField, Range(0.5f, 5f)] private float damageDelay = 1f;
        public float DamageDelay { get => damageDelay; set => damageDelay = value; }

        void OnEnable()
        {
            PayloadType = EPayloadType.Delayed;
        }
    }
}

