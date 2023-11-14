using System;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "EncounterConfig_Difficulty_Num", menuName = "Status Unknown/Gameplay/Combat/Encounter", order = 20)]
    public class EnemyEncounterConfigSO : ScriptableObject
    {
        [field:SerializeField, HideInInspector] public EnemyData[] EnemyDatas { get ; set ; }
    }

    [Serializable]
    public class EnemyData
    {
        public EnemyConfigSO enemyConfig;
        public Vector3 position; 
    }
}
