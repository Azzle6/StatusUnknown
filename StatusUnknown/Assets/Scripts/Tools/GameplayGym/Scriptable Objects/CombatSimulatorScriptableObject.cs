using System;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "CombatSimualator_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Simulator", order = 50)]
    public class CombatSimulatorScriptableObject : ScriptableObject
    {
        public AbilityConfigScriptableObject[] abilities;
        private Action<Payload> OnExecuteFinish;
        private int index; 

        internal EDamageType GetRootValue()
        {
            index++; 
            return abilities[0].GetDamageType(); 
        }

        internal EDamageType GetNextValue()
        {
            return abilities[index++].GetDamageType();
        }
    }
}
