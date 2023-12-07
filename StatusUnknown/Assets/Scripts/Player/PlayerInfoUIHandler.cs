
using Augment;
using UI;

namespace Player
{
    using System.Collections.Generic;
    using Core.VariablesSO.VariableTypes;
    using UnityEngine;
    using UnityEngine.UIElements;
    using DG.Tweening;
    using Weapon;
    using Core;
    
    public class PlayerInfoUIHandler : MonoSingleton<PlayerInfoUIHandler>
    {
        [SerializeField] private FloatVariableSO playerHealth;
        [SerializeField] private IntVariableSO medikitAmount;
        [SerializeField] private WeaponVariableSO[] weaponVariableSO;
        [SerializeField] private FloatVariableSO[] weaponAmmoVariableSO;
        [SerializeField] private VisualTreeAsset popupUIDocument;
        private VisualElement popUpZone;
        private List<VisualElement> augmentIcon;
        
        private int tempMedikitAmount;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private UIDocument playerInfoUIDocument;
        private VisualElement healthBar;
        private VisualElement weapon1Icon;
        private VisualElement weapon2Icon;
        private VisualElement ammoRoot1;
        private VisualElement ammoRoot2;
        private Label medikitCount;
        private Label weapon1AmmoCount;
        private Label weapon2AmmoCount;
        private VisualElement medikitIcon;
        private float[] currentMaxAmmo = new float[2];
        [Header("PopUp")]
        [SerializeField] private float popUpScaleTime;
        [SerializeField] private float popUpStayTime;
        [SerializeField] private float popUpFadeTime;
        [SerializeField] private int popUpMaxCount = 3;
        
        
        
        private void Awake()
        {
            healthBar = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("HealthBar");
            medikitCount = playerInfoUIDocument.rootVisualElement.Q<Label>("MedikitCount");
            medikitIcon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("MedikitIcon");
            weapon1Icon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("Weapon1Icon");
            weapon2Icon = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("Weapon2Icon");
            popUpZone = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("MidRightZone");
            weapon1AmmoCount = playerInfoUIDocument.rootVisualElement.Q<Label>("Weapon1AmmoCount");
            weapon2AmmoCount = playerInfoUIDocument.rootVisualElement.Q<Label>("Weapon2AmmoCount");
            ammoRoot1 = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("AmmoWeapon1");
            ammoRoot2 = playerInfoUIDocument.rootVisualElement.Q<VisualElement>("AmmoWeapon2");
            augmentIcon = new List<VisualElement>();
            for (int x = 0; x < 5; x++)
                augmentIcon.Add(playerInfoUIDocument.rootVisualElement.Q<VisualElement>($"Augment{x}"));
            weaponVariableSO[0].RegisterOnValueChanged(weapon => UpdateWeaponIcon(0, weapon));
            weaponVariableSO[1].RegisterOnValueChanged(weapon => UpdateWeaponIcon(1, weapon));
            weaponAmmoVariableSO[0].RegisterOnValueChanged(UpdateWeapon1AmmoCount);
            weaponAmmoVariableSO[1].RegisterOnValueChanged(UpdateWeapon2AmmoCount);
            playerHealth.RegisterOnValueChanged(UpdateHealthBar);
            medikitAmount.RegisterOnValueChanged(UpdateMedikitCount);
            tempMedikitAmount = medikitAmount.Value;
            UpdateHealthBar(playerHealth.Value);
            InitMedikitCount(medikitAmount.Value);
            UpdateWeaponIcon(0, weaponVariableSO[0].Value);
            UpdateWeaponIcon(1, weaponVariableSO[1].Value);
     


        }

        public void Hide()
        {
            playerInfoUIDocument.rootVisualElement.style.display = DisplayStyle.None;
        }

        public void Show()
        {
            playerInfoUIDocument.rootVisualElement.style.display = DisplayStyle.Flex;
        }
        
        private void UpdateWeaponIcon(int index, Weapon weapon)
        {
            VisualElement weaponIcon = index == 0 ? weapon1Icon : weapon2Icon;
            Label weaponAmmoCount = index == 0 ? weapon1AmmoCount : weapon2AmmoCount;
            VisualElement ammoRoot = index == 0 ? ammoRoot1 : ammoRoot2;

            weaponIcon.style.backgroundImage = weapon.weaponSprite.texture;
            weaponIcon.style.width = weapon.weaponSprite.texture.width;
            weaponIcon.style.height = weapon.weaponSprite.texture.height;
            ammoRoot.style.visibility = weaponVariableSO[index].Value.weaponType == WeaponType.RANGED ? Visibility.Visible : Visibility.Hidden;

            if (weaponVariableSO[index].Value.weaponType == WeaponType.RANGED)
            {
                RangedWeapon rangedWeapon = (RangedWeapon) weaponVariableSO[index].Value;
                currentMaxAmmo[index] = rangedWeapon.GetMagazineSize();
                weaponAmmoCount.text = $"{currentMaxAmmo[index]} / {currentMaxAmmo[index]}";
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
            if (medikitAmount.Value < tempMedikitAmount)
            {
                medikitIcon.transform.scale = Vector3.zero;
                Vector3 currentScale = medikitIcon.transform.scale;
                DOTween.To(() => currentScale, x => medikitIcon.transform.scale = x, Vector3.one, playerStat.medikitCooldown);
            }
            tempMedikitAmount = medikitAmount.Value;
         
        }
        
        public void ShowPopup(PopUpData popUpData)
        {
            VisualElement tempPopup = popupUIDocument.CloneTree();
            tempPopup.Q<VisualElement>("IconPopUp").style.backgroundImage = popUpData.icon.texture;
            tempPopup.Q<Label>("LabelPopUp").text = popUpData.title;
            popUpZone.Add(tempPopup);
            if (popUpMaxCount < popUpZone.childCount)
            {
                popUpZone.RemoveAt(0);
            }
            tempPopup.transform.scale = Vector3.zero;
            Vector3 currentScale = tempPopup.transform.scale;
            DOTween.To(() => currentScale, x => tempPopup.transform.scale = x, Vector3.one, popUpScaleTime)
                .OnComplete(() =>
                {
                    tempPopup.style.opacity = 1;
                    DOTween.To(() => tempPopup.style.opacity.value, x => tempPopup.style.opacity = x, 0, popUpFadeTime)
                        .SetDelay(popUpStayTime)
                        .OnComplete(() =>
                        {
                            tempPopup.RemoveFromHierarchy();
                        });
                });
        }

        public void ReceiveAugmentStat(AugmentStat augmentStat)
        {
            if (augmentIcon[augmentStat.augmentSlot].style.backgroundImage == augmentStat.augmentSprite)
            {
                UseAugment(augmentStat);
            }
            else
            {
                SwitchAugmentStat(augmentStat);
            }
        }
        
        public void SwitchAugmentStat(AugmentStat augmentStat)
        {
            augmentIcon[augmentStat.augmentSlot].style.backgroundImage = augmentStat.augmentSprite;

        }

        public void UseAugment(AugmentStat augmentStat)
        {
            augmentIcon[augmentStat.augmentSlot].style.unityBackgroundImageTintColor = Color.black;
            DOTween.To(() => augmentIcon[augmentStat.augmentSlot].style.unityBackgroundImageTintColor.value, x => augmentIcon[augmentStat.augmentSlot].style.unityBackgroundImageTintColor = x, Color.white, augmentStat.augmentCooldown);
        }
    }
}


