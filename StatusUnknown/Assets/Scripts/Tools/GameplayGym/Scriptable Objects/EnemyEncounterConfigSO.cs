using System;
using System.Collections.Generic;
using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "EncounterConfig_Difficulty_Num", menuName = "Status Unknown/Gameplay/Combat/Encounter", order = 20)]
    public class EnemyEncounterConfigSO : ScriptableObject
    {
        public EnemyData[] EnemyDatas { get ; set ; }
    }

    [Serializable]
    public class EnemyData
    {
        public GameObject EnemyPrefab;
        public Vector3 Positions; 
    }
}
