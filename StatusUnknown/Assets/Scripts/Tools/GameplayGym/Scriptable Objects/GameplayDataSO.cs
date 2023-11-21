using UnityEngine;

[CreateAssetMenu(fileName = "Gameplay Data", menuName = "Status Unknown/Gameplay/Data", order = 100)]
public class GameplayDataSO : ScriptableObject
{
    public int TotalDamageDone { get; set; }
    [field:SerializeField] public bool MustRefreshAreasObj { get; set; }
    [field: SerializeField] public bool ExitedPlayMode { get; set; }

    public void Init()
    {
        TotalDamageDone = 0;    
    }
}
