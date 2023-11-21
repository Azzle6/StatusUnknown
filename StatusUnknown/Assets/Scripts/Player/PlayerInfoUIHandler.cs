using System;
using DG.Tweening;

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
        private int tempMedikitAmount;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private UIDocument playerInfoUIDocument;
        private VisualElement healthBar;
        private VisualElement weapon1Icon;
        private VisualElement weapon2Icon;
        private Label medikitCount;
        private VisualElement medikitIcon;

        private void Start()
        {
            healthBar = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("HealthBar");
            medikitCount = playerInfoUIDocument.rootVisualElement.Q<Label>("MedikitCount");
            medikitIcon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("MedikitIcon");
            weapon1Icon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("Weapon1Icon");
            weapon2Icon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("Weapon2Icon");
            weaponVariableSO[0].RegisterOnValueChanged(UpdateWeapon1Icon);
            weaponVariableSO[1].RegisterOnValueChanged(UpdateWeapon2Icon);
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
        }
        
        private void UpdateWeapon2Icon(Weapon.Weapon weapon)
        {
            weapon2Icon.style.backgroundImage = weapon.weaponSprite.texture;
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
            if (  medikitAmount.Value < tempMedikitAmount)
            {
                medikitIcon.transform.scale = Vector3.zero;
                Vector3 currentScale = medikitIcon.transform.scale;
                DOTween.To(() => currentScale, x => medikitIcon.transform.scale = x, Vector3.one, playerStat.medikitCooldown);
            }
            tempMedikitAmount = medikitAmount.Value;
         
        }
        
    }
}


