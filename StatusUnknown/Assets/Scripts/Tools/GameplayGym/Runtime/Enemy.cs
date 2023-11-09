using StatusUnknown.CoreGameplayContent;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamageable
{
    [Space, SerializeField] private EnemyConfigScriptableObject enemySO;
    [Space, SerializeField] private bool overrideMaxHP = false;
    [SerializeField, Range(10, 500)] private int maxHP_Override = 50; 
    private int maxHP; 
    private int currentHP;

    private void OnEnable()
    {
        if (enemySO == null) return; 

        Init();        
    }

    private void Init()
    {
        maxHP = overrideMaxHP ? maxHP_Override : enemySO.MaxHP;
        currentHP = maxHP; 
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage; 
        if (currentHP <= 0 && gameObject)
        {
            Destroy(gameObject); 
        }
    }

    //[Button]
    public void SaveOverridenMaxHPToCurrentSO()
    {
        enemySO.OverrideMaxHP(maxHP_Override); 
    }

}
