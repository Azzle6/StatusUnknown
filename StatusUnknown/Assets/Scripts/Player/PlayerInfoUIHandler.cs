using System;
using DG.Tweening;
using Weapon;

namespace Player
{
    using Core.VariablesSO.VariableTypes;
    using UnityEngine;
    using UnityEngine.UIElements;
    
    
    public class PlayerInfoUIHandler : MonoBehaviour
    {
        [SerializeField] private FloatVariableSO playerHealth;
        [SerializeField] private IntVariableSO medikitAmount;
        [SerializeField] private WeaponVariableSO[] weaponVariableSO;
        [SerializeField] private FloatVariableSO[] weaponAmmoVariableSO;
        private int tempMedikitAmount;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private UIDocument playerInfoUIDocument;
        private VisualElement healthBar;
        private VisualElement weapon1Icon;
        private VisualElement weapon2Icon;
        private Label medikitCount;
        private Label weapon1AmmoCount;
        private Label weapon2AmmoCount;
        private VisualElement medikitIcon;
        private float[] currentMaxAmmo = new float[2];

        private void Start()
        {
            healthBar = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("HealthBar");
            medikitCount = playerInfoUIDocument.rootVisualElement.Q<Label>("MedikitCount");
            medikitIcon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("MedikitIcon");
            weapon1Icon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("Weapon1Icon");
            weapon2Icon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("Weapon2Icon");
            weapon1AmmoCount = playerInfoUIDocument.rootVisualElement.Q<Label>("Weapon1AmmoCount");
            weapon2AmmoCount = playerInfoUIDocument.rootVisualElement.Q<Label>("Weapon2AmmoCount");
            weaponVariableSO[0].RegisterOnValueChanged(UpdateWeapon1Icon);
            weaponVariableSO[1].RegisterOnValueChanged(UpdateWeapon2Icon);
            weaponAmmoVariableSO[0].RegisterOnValueChanged(UpdateWeapon1AmmoCount);
            weaponAmmoVariableSO[1].RegisterOnValueChanged(UpdateWeapon2AmmoCount);
            playerHealth.RegisterOnValueChanged(UpdateHealthBar);
            medikitAmount.RegisterOnValueChanged(UpdateMedikitCount);
            tempMedikitAmount = medikitAmount.Value;
            UpdateHealthBar(playerHealth.Value);
            InitMedikitCount(medikitAmount.Value);
            UpdateWeapon1Icon(weaponVariableSO[0].Value);
            UpdateWeapon2Icon(weaponVariableSO[1].Value);
            
        }
        
        private void UpdateWeapon1Icon(Weapon.Weapon weapon)
        {
            weapon1Icon.style.backgroundImage = weapon.weaponSprite.texture;
            weapon1AmmoCount.visible = weaponVariableSO[0].Value.weaponType == WeaponType.RANGED;

            if (weaponVariableSO[0].Value.weaponType == WeaponType.RANGED)
            {
                RangedWeapon rangedWeapon = (RangedWeapon) weaponVariableSO[0].Value;
                currentMaxAmmo[0] = rangedWeapon.GetMagazineSize();
                weapon1AmmoCount.text = $"{currentMaxAmmo[0]} / {currentMaxAmmo[0]}";
            }
            
        }
        
        private void UpdateWeapon2Icon(Weapon.Weapon weapon)
        {
            weapon2Icon.style.backgroundImage = weapon.weaponSprite.texture;
            weapon2AmmoCount.visible = weaponVariableSO[1].Value.weaponType == WeaponType.RANGED;
            
            if (weaponVariableSO[1].Value.weaponType == WeaponType.RANGED)
            {
                RangedWeapon rangedWeapon = (RangedWeapon) weaponVariableSO[1].Value;
                currentMaxAmmo[1] = rangedWeapon.GetMagazineSize();
                weapon2AmmoCount.text = $"{currentMaxAmmo[1]} / {currentMaxAmmo[1]}";
            }
        }
        
        private void UpdateWeapon1AmmoCount(float newAmmo)
        {
            weapon1AmmoCount.text = $"{newAmmo} / {currentMaxAmmo[0]}";
        }
        
        private void UpdateWeapon2AmmoCount(float newAmmo)
        {
            weapon2AmmoCount.text = $"{newAmmo} / {currentMaxAmmo[1]}";
        }

        private void UpdateHealthBar(float newHealth)
        {
            Vector3 newScale = new Vector3(newHealth / playerStat.maxHealth, 1, 1);
            DOTween.To(() => healthBar.transform.scale, x => healthBar.transform.scale = x, newScale, 0.1f);
        }
        
        private void InitMedikitCount(int newMedikitCount)
        {
            medikitCount.text = newMedikitCount.ToString();
        }
        
        private void UpdateMedikitCount(int newMedikitCount)
        {
            medikitCount.text = newMedikitCount.ToString();
            Debug.Log("Medikit amount: " + medikitAmount.Value + " + tempMedikitAmount: " + tempMedikitAmount);
            if (medikitAmount.Value < tempMedikitAmount)
            {
                medikitIcon.transform.scale = Vector3.zero;
                Vector3 currentScale = medikitIcon.transform.scale;
                DOTween.To(() => currentScale, x => medikitIcon.transform.scale = x, Vector3.one, playerStat.medikitCooldown);
            }
            tempMedikitAmount = medikitAmount.Value;
         
        }
        
    }
}


