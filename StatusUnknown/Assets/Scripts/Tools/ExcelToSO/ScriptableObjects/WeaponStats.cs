using UnityEngine;

public enum GameplayType { Melee, Distance };

[CreateAssetMenu(fileName = "Weapon", menuName = "Status Unknown/Systems/weapon")]
public class WeaponStats : ScriptableObject
{
    public string ID;
    public GameplayType Type;
    [TextArea(3, 20)] public string Description;
    public float FireRate;
    public int Slots_Amount;
}
