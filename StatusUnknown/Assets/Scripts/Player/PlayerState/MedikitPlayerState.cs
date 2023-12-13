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
        private bool medikitInCD;
        public override void OnStateEnter()
        {
            if (medikitAmount.Value <= 0 || medikitInCD)
                return;
            
            playerHealth.Heal(stat.medikitHealAmount);
            medikitAmount.Value--;

            StartCoroutine(MedikitCD());
            playerStateInterpretor.RemoveStateCheck("MedikitPlayerState");
        }
        
        private IEnumerator MedikitCD()
        {
            medikitInCD = true;
            yield return new WaitForSeconds(stat.medikitCooldown);
            medikitInCD = false;
        }
        
        

        public override void OnStateExit()
        {
            
        }
    }

}
