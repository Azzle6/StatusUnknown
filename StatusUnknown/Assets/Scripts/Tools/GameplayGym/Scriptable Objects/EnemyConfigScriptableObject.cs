using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "EnemyConfig_Difficulty_Num", menuName = "Status Unknown/Gameplay/Combat/Enemy")]
    public class EnemyConfigScriptableObject : ScriptableObject
    {
        [SerializeField, Range(10, 500)] private int maxHP = 50; 
        public int MaxHP { get => maxHP ; }

        public void OverrideMaxHP(int newValue) { maxHP = newValue; }
    }
}
