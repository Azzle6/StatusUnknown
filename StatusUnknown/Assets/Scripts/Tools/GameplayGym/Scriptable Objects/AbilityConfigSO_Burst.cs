using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "Ability_Burst_Name", menuName = "Status Unknown/Gameplay/Combat/Ability Burst")]
    public class AbilityConfigSO_Burst : AbilityConfigSO_Base
    {
        void OnEnable()
        {
            PayloadType = EPayloadType.Burst;
        }
    }
}

