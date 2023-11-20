using Core.VariablesSO.VariableTypes;

namespace Player
{
    using UnityEngine;

    public class PlayerHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private PlayerStat stat;
        private FloatVariableSO playerHealth;
        private FloatVariableSO playerArmor;


        private void Awake()
        {
            playerHealth.Value = stat.maxHealth;
        }

        public void TakeDamage(float damage, Vector3 force)
        {
            playerHealth.Value -= damage;
            Debug.Log("Player took " + damage + " damage");
        }
    
        public void Heal(float amount)
        {
            if (playerHealth.Value >= stat.maxHealth)
            {
                playerHealth.Value = stat.maxHealth;
            }
            else
            {
                playerHealth.Value += amount;
            }
        }
    }

}
