using StatusUnknown.CoreGameplayContent;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability_Delayed_Name", menuName = "Status Unknown/Gameplay/Combat/Ability Delayed")]
public class AbilityConfigSO_Delayed : AbilityConfigSO_Base
{
    [SerializeField, Range(0.5f, 5f)] private float damageDelay = 1f;
    public float DamageDelay => damageDelay;

    void OnEnable()
    {
        PayloadType = EPayloadType.Delayed;
    }
}
