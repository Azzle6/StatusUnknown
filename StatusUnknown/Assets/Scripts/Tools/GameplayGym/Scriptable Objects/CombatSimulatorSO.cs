using System;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "CombatSimualator_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Simulator", order = 50)]
    public class CombatSimulatorSO : ScriptableObject
    {
        public AbilityConfigSO_Base[] abilitiesConfig;

        internal int GetAbilitiesArrayLength() { return abilitiesConfig.Length; } 

        internal (AbilityInfos infos, AbilityConfigSO_Base so) GetRootAbilityData()
        {
            if (abilitiesConfig == null) return default;

            return abilitiesConfig[0].GetAbilityData(); 
        }

        internal (AbilityInfos infos, AbilityConfigSO_Base so) GetAbilityDataAtIndex(int i)
        {
            return abilitiesConfig[i].GetAbilityData();
        }


    }
}
