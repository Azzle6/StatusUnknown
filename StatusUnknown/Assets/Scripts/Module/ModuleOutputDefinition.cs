namespace Module
{
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "CustomAssets/Definitions/ModuleOutputDefinition", fileName = "ModuleOutputDefinition")]
    public class ModuleOutputDefinition : ScriptableObject
    {
        public E_ModuleOutput output;
        public Sprite icon;
        public string description;
    }
    
    public enum E_ModuleOutput
    {
        ON_SPAWN,
        ON_HIT,
        ON_TICK,
        ON_END
    }
}
