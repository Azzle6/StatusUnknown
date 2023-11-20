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
            if (medikitAmount.Value > 0)
            {
                playerHealth.Heal(stat.medikitHealAmount);
                medikitAmount.Value--;
            }
            
            playerStateInterpretor.RemoveState(PlayerStateType.ACTION);
        }

        public override void OnStateExit()
        {
            
        }
    }

}
