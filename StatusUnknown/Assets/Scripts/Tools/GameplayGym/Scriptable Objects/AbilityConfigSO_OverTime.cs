using StatusUnknown.CoreGameplayContent;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability_OverTime_Name", menuName = "Status Unknown/Gameplay/Combat/Ability Over Time")]
public class AbilityConfigSO_OverTime : AbilityConfigSO_Base
{
    [SerializeField, Range(2, 20)] private int tickAmount = 3;
    [SerializeField, Range(0.1f, 2f)] private float tickDelay = 0.5f;
    public int TickAmount { get => tickAmount; set => tickAmount = value; }
    public float TickDelay { get => tickDelay; set => tickDelay = value; }  

    void OnEnable()
    {
        PayloadType = EPayloadType.OverTime;
    }
}
