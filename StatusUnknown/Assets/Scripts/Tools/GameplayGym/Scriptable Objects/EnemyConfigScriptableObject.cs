using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "EnemyConfig_Difficulty_Num", menuName = "Status Unknown/Gameplay/Combat/Enemy")]
    public class EnemyConfigScriptableObject : ScriptableObject
    {
        public int maxHP;
        public int CurrentHP { get; private set; }
    }
}
