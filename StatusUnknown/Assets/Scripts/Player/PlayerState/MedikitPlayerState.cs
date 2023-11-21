using Core.VariablesSO.VariableTypes;

namespace Player
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    
    
    public class MedikitPlayerState : PlayerState
    {
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private IntVariableSO medikitAmount;
        [SerializeField] private PlayerStat stat;
        public override void OnStateEnter()
        {
            if (medikitAmount.Value <= 0)
                return;
            
            playerHealth.Heal(stat.medikitHealAmount);
            medikitAmount.Value--;
            
            
            playerStateInterpretor.RemoveStateCheck("MedikitPlayerState");
        }
        
        

        public override void OnStateExit()
        {
            
        }
    }

}
