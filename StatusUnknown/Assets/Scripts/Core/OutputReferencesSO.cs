namespace Core
{
    using Helpers;
    using UnityEngine;
    
    [CreateAssetMenu(menuName = "CustomAssets/IconsReferencement", fileName = "IconsReferencement")]
    public class OutputReferencesSO : ScriptableObject
    {
        public ModuleOutputReferencesDictionary moduleOutputReferences;
        public WeaponOutputReferencesDictionary weaponOutputReferences;
    }
}
