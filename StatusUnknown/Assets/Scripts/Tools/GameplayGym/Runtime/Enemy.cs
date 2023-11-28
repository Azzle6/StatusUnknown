using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace StatusUnknown.Content
{
    [ExecuteAlways]
    public class Enemy : MonoBehaviour, IDamageable
    {
        [Header("General")]
        [Space, SerializeField] private EnemyConfigSO enemySO;
        [SerializeField] private GameplayDataSO gameplayDataSO; 
        [SerializeField] private GameObject enemyObj;
        [SerializeField] private GameObject damagePrinterObj;
        private MeshRenderer objMeshRenderer; 

        public EnemyConfigSO EnemyConfigSO { get => enemySO; set => enemySO = value; }

        [Header("UI")]
        [SerializeField] private Image panelImage;
        [SerializeField] private Slider enemyHP_UI;
        [SerializeField] private TMP_Text enemyHPText_UI;
        [SerializeField] private TMP_Text enemyTotalDamage_UI;
        private int damageCounter; 

        [Header("-- DEBUG --")]
        [Space, SerializeField] private bool overrideMaxHP = false;
        [SerializeField, Range(10, 1000)] private int maxHP_Override = 250;
        private int maxHP;
        private int currentHP;
        private bool isDead; 

        private void OnEnable()
        {
            if (enemySO == null) return;

            GameplayManager.OnSimulationDone += GetFinalPayload; 
            Init();
        }

        private void OnDisable()
        {
            GameplayManager.OnSimulationDone -= GetFinalPayload;
        }

        private void Init()
        {
            maxHP = overrideMaxHP ? maxHP_Override : enemySO.MaxHP;
            currentHP = maxHP;
            enemyHP_UI.maxValue = maxHP;
            enemyHP_UI.value = maxHP;
            enemyHPText_UI.text = maxHP.ToString();
            panelImage.color = enemySO.EnemyColor;

            damagePrinterObj.SetActive(false);
            objMeshRenderer = GetComponentInChildren<MeshRenderer>();
            objMeshRenderer.enabled = true; 
        }

        public void TakeDamage(int damage)
        {
            if (isDead) return; 

            damageCounter += damage; 
            currentHP -= damage;
            enemyHP_UI.value = currentHP;
            enemyHPText_UI.text = currentHP.ToString(); ;

            if (currentHP <= 0 && gameObject)
            {
                currentHP = 0;
                objMeshRenderer.enabled = false; 
                isDead = true; 
            }
        }

        //[Button]
        public void SaveOverridenMaxHPToCurrentSO()
        {
            enemySO.OverrideMaxHP(maxHP_Override);
        }

        private void GetFinalPayload()
        {
            damagePrinterObj.SetActive(true);
            enemyTotalDamage_UI.SetText($"{damageCounter} ({((float)damageCounter / maxHP) * 100}% of max HP)");

            gameplayDataSO.TotalDamageDone += damageCounter; 
        }

    }
}
