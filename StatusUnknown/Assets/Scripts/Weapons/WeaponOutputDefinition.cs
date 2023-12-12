namespace Weapons
{
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "CustomAssets/Definitions/WeaponOutputDefinition", fileName = "WeaponOutputDefinition")]
    public class WeaponOutputDefinition : ScriptableObject
    {
        public E_WeaponOutput output;
        public Sprite icon;
        public string description;
    }
    
    public enum E_WeaponOutput
    {
        ON_HIT,
        ON_SPAWN,
        ON_RELOAD,
        ON_HIT_FULL_CHARGED,
        ON_HIT_FIRST_ATTACK,
        ON_HIT_SECOND_ATTACK,
        ON_HIT_THIRD_ATTACK
    }
}
