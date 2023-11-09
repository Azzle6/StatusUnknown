using System;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "CombatSimualator_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Simulator", order = 50)]
    public class CombatSimulatorScriptableObject : ScriptableObject
    {
        public AbilityConfigScriptableObject[] abilities;

        internal int GetAbilitiesArrayLength() { return abilities.Length; } 

        internal EDamageType GetRootValue()
        {
            if (abilities == null) return default;

            return abilities[0].GetDamageType(); 
        }

        internal EDamageType GetValueAtIndex(int i)
        {
            return abilities[i].GetDamageType();
        }
    }
}
