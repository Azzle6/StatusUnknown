using UnityEngine;

[CreateAssetMenu(fileName = "Player Reputation", menuName = "Status Unknown/Gameplay/Player Reputation")]
public class PlayerReputationSO : ScriptableObject
{
    [field:SerializeField, Range(0, 500)] public int PlayerReputation {  get; set; }  
}
