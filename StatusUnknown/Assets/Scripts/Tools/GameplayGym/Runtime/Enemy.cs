using Sirenix.OdinInspector;
using StatusUnknown.CoreGameplayContent;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[ExecuteAlways]
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("General")]
    [Space, SerializeField] private EnemyConfigSO enemySO;

    [Header("UI")]
    [SerializeField] private Image panelImage; 
    [SerializeField] private Slider enemyHP_UI;
    [SerializeField] private TMP_Text enemyHPText_UI;

    [Header("-- DEBUG --")]
    [Space, SerializeField] private bool overrideMaxHP = false;
    [SerializeField, Range(10, 1000)] private int maxHP_Override = 250;
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
        enemyHP_UI.maxValue = maxHP; 
        enemyHP_UI.value = maxHP;
        enemyHPText_UI.text = maxHP.ToString();
        panelImage.color = enemySO.EnemyColor; 
    }

    public void TakeDamage(int damage)
    {
        currentHP -= damage;
        enemyHP_UI.value = currentHP;
        enemyHPText_UI.text = currentHP.ToString(); ;

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
