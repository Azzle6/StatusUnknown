using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "EncounterConfig_Difficulty_Num", menuName = "Status Unknown/Gameplay/Combat/Encounter", order = 20)]
    public class EnemyEncounterConfigScriptableObject : ScriptableObject
    {
        public EnemyConfigScriptableObject[] Enemies;
    }
}
