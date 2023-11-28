using UnityEngine;
using StatusUnknown.Utils.AssetManagement;


namespace StatusUnknown.Content
{
    [ManageableData]
    [CreateAssetMenu(fileName = "EnemyConfig_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Enemy", order = 4)]
    public class EnemyConfigSO : ScriptableObject
    {
        [SerializeField] private Color enemyColor = new Color32(130, 10, 100, 255); 
        [SerializeField, Range(10, 1000)] private int maxHP = 100;
        [field: SerializeField] public int Type_ID { get; set; }

        public int MaxHP { get => maxHP ; }
        public Color EnemyColor { get => enemyColor; }

        public void OverrideMaxHP(int newValue) { maxHP = newValue; }
    }
}
