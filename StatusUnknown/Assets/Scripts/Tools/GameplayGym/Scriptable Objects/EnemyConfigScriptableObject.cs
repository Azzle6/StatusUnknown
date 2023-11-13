using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "EnemyConfig_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Enemy")]
    public class EnemyConfigScriptableObject : ScriptableObject
    {
        [SerializeField] private Color enemyColor = new Color32(130, 10, 100, 255); 
        [SerializeField, Range(10, 1000)] private int maxHP = 100; 
        public int MaxHP { get => maxHP ; }
        public Color EnemyColor { get => enemyColor; }

        public void OverrideMaxHP(int newValue) { maxHP = newValue; }
    }
}
