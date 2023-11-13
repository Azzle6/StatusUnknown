using StatusUnknown.CoreGameplayContent;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability_Burst_Name", menuName = "Status Unknown/Gameplay/Combat/Ability Burst")]
public class AbilityConfigSO_Burst : AbilityConfigSO_Base
{
    void OnEnable()
    {
        payloadType = EPayloadType.Burst;
    }
}
