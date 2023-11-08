using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "Ability_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Ability")]
    public class AbilityConfigScriptableObject : ScriptableObject
    {
        public EAbilityType AbilityType = EAbilityType.Offense; 
        public EDamageType DamageType = EDamageType.DOT;
        public float Cooldown = 1.5f;

        private void SendEvent(ChangeEvent<EAbilityType> evt)
        {
            Debug.Log($"value changed from {evt.previousValue} to {evt.newValue}"); 
        }

        public IBinding binding { get; set; }
        public string bindingPath { get; set; }

        public void SetValueWithoutNotify(EAbilityType newValue)
        {
            // 
        }
    }
}
