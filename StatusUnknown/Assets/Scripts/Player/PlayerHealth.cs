using System.Collections;
using Core.VariablesSO.VariableTypes;

namespace Player
{
    using UnityEngine;

    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerStat stat;
        [SerializeField] private FloatVariableSO playerHealth;
        [SerializeField] private FloatVariableSO playerArmor;
        private Coroutine medikitCD;


        private void Awake()
        {
            playerHealth.Value = stat.maxHealth;
        }

        public void TakeDamage(float damage, Vector3 force)
        {
            playerHealth.Value -= damage;
            //Debug.Log("Player took " + damage + " damage");
        }

        private IEnumerator MedikitCD()
        {
            yield return new WaitForSeconds(stat.medikitCooldown);
            medikitCD = null;
        }
    
        public void Heal(float amount)
        {
            if (medikitCD != null)
                return;
            
            if (playerHealth.Value + amount >= stat.maxHealth)
            {
                playerHealth.Value = stat.maxHealth;
            }
            else
            {
                playerHealth.Value += amount;
            }
            
            medikitCD = StartCoroutine(MedikitCD());
        }
    }

}
