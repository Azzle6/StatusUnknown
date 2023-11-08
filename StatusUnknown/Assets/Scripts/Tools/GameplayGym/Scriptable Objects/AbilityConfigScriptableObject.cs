using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "Ability_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Ability")]
    public class AbilityConfigScriptableObject : ScriptableObject
    {
        public EAbilityType AbilityType = EAbilityType.Offense; 
        public EDamageType DamageType = EDamageType.DOT;
        public float Cooldown = 1.5f;

        public EDamageType GetDamageType() => DamageType; 
    }
}
