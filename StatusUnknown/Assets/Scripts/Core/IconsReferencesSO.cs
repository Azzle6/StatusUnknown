namespace Core
{
    using System;
    using Helpers;
    using Module.Definitions;
    using UnityEngine;
    using Weapons;
    
    [CreateAssetMenu(menuName = "CustomAssets/IconsReferencement", fileName = "IconsReferencement")]
    public class IconsReferencesSO : ScriptableObject
    {
        public IconReferencesDictionary<E_ModuleOutput> moduleOutputReferences;
        public IconReferencesDictionary<E_WeaponOutput> weaponOutputReferences;
    }

    [Serializable]
    public struct IconReferences<T>
    {
        public T reference;
        public Sprite icon;
    }
}
