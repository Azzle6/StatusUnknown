using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using StatusUnknown.Content;
using System;
using StatusUnknown.Tools;

public class WeaponEditorWindow : MonoBehaviour
{

    /* [SerializeField, VerticalGroup("Base/right/weapon", GroupName = "WEAPON", VisibleIf = "@Items == Item.Weapon"), LabelWidth(CoreToolsStrings.LABEL_SIZE_SMALL), LabelText("@$value.weaponName")] private Weapon Weapon = new Weapon();
    [VerticalGroup("Base/right/weapon", GroupName = "WEAPON"), SerializeField, InfoBox("@GetWeaponDescription()", Icon = SdfIconType.Info, InfoMessageType = InfoMessageType.Info), HideLabel] string weaponInfos;
    private string GetWeaponDescription() => string.IsNullOrEmpty(Weapon.Description) ? "No Weapon Description has been written yet" : Weapon.Description; */


    [Serializable]
    internal class Weapon // load or create new weapon
    {
        #region Odin Property Processor
        [HideInInspector] public const bool SHOW_METADATA = false;
        #endregion

        CoreContentStrings s = new CoreContentStrings();

        [SerializeField, LabelWidth(700)] string weaponName = "New_Weapon_Name";
        [SerializeField, PreviewField] protected Sprite imageUI;
        [SerializeField, PreviewField] protected GameObject weaponPrefab; // previz
        [SerializeField, PreviewField] protected AudioClip shootDefaultSound; // previz
        [SerializeField, PreviewField] protected Sprite shootDefaultFXTexture; // previz
        [SerializeField, PreviewField] protected Animation shootDefaultAnimation; // previz
        [SerializeField, TextArea(5, 10)] protected string description;
        [SerializeField, PropertyRange(0, 100)] protected int unlockCost = 30;
        [PropertySpace(10), SerializeField, PropertyRange(0.1f, 10f)] private float attackSpeed = 1.5f;
        [SerializeField, PropertyRange(0, 100)] private int damage = 5;

        public int Damage { get => damage; }
        public float AttackSpeed { get => attackSpeed; }
        public string Description { get => description; }
    }

    internal class Bow : Weapon
    {
        [SerializeField, PropertyRange(1f, 10f)] private float fireRange = 1.5f;
    }

    internal class Sword : Weapon
    {
        [SerializeField, PropertyRange(0f, 1f)] private float stunChancePercent;
    }

    internal class WeaponPropertyProcessor<T> : OdinPropertyProcessor<T> where T : Weapon
    {
        public override void ProcessMemberProperties(List<InspectorPropertyInfo> propertyInfos)
        {
            for (int i = 0; i < propertyInfos.Count; i++)
            {
                //Debug.Log("property name : " + propertyInfos[i].PropertyName);
            }

            propertyInfos.AddValue("DPS",
                (ref Weapon w) => w.AttackSpeed * w.Damage,
                (ref Weapon w, float value) => { },
                new BoxGroupAttribute("Balancing Data"),
                new EnableIfAttribute("SHOW_METADATA", true));
        }
    }
}
