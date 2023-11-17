using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace StatusUnknown.CoreGameplayContent.Editors
{
    // [CustomPropertyDrawer(typeof(AbilityConfigScriptableObject), true)]
    public class AbilityConfigScriptableObjectPropertyDrawer : PropertyDrawer
    {
        private VisualElement ContentPanel;
        private UnityEngine.UIElements.Label CaretLabel;
        private bool isExpanded = true;
        TextField textField_DamageType = new TextField();
        EnumField enumField_DamageType = new EnumField(); 
        HelpBox helpBox_DamageType = new HelpBox();

        private EScriptableType newValue_ScriptableType = EScriptableType.NONE;
        private EScriptableType previousValue_ScriptableType = EScriptableType.NONE;

        private EPayloadType newValue_DamageType = EPayloadType.Burst;
        private EPayloadType previousValue_DamageType = EPayloadType.Burst;

        readonly string[] hBTextFields = new string[]
        {
            "Instant application of damage",
            "Damage Over Time",
            "Damage applied after a delay"
        };

        Button createButton = new Button();

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement inspector = new VisualElement();
            inspector.AddToClassList("panel");

            return BuildUI(inspector, property);
        }

        private VisualElement BuildUI(VisualElement RootElement, SerializedProperty property)
        {
            EnumValueTracker.OnValueChanged_EScriptableType += SetCreationText;

            SerializedProperty sP = property.serializedObject.FindProperty("scriptableObjectType");
            Enum.TryParse(sP.enumDisplayNames[sP.enumValueIndex], out newValue_ScriptableType);
            if (textField_DamageType != null && string.IsNullOrEmpty(textField_DamageType.text))
            {
                SetCreationText(newValue_ScriptableType); 
            }

            // DOES NOT WORK -- can't parse EnumField to EDamageType or vice versa..
            // RegisterValueChanged_EDamageType(newValue_DamageType, helpBox_DamageType);   

            VisualElement titleContainer = new VisualElement();
            titleContainer.AddToClassList("align-horizontal");
            CaretLabel = new UnityEngine.UIElements.Label(">");
            CaretLabel.style.fontSize = 18;
            CaretLabel.AddToClassList(isExpanded ? "rotate-90" : "rotate-0");
            titleContainer.Add(CaretLabel);
            UnityEngine.UIElements.Label title = new UnityEngine.UIElements.Label("Ability Configuration");
            titleContainer.AddToClassList("header");

            titleContainer.Add(title);
            titleContainer.RegisterCallback<ClickEvent>(HandleTitleClick);

            RootElement.Add(titleContainer);

            if (property.objectReferenceValue == null)
            {
                ContentPanel = BuildNoAbilityConfigBox(RootElement, property);
                RootElement.Add(ContentPanel);
            }
            else
            {
                ContentPanel = BuildAbilityConfigBox(RootElement, property);
                RootElement.Add(ContentPanel);
            }

            ContentPanel.AddToClassList(isExpanded ? "expanded" : "collapsed");

            return RootElement;
        }

        private VisualElement BuildNoAbilityConfigBox(VisualElement rootElement, SerializedProperty property)
        {
            VisualElement noShootConfigBox = new VisualElement();
            noShootConfigBox.name = "no-ability-config-box";

            UnityEngine.UIElements.Label noShootConfigLabel = new UnityEngine.UIElements.Label("No Ability Config Exists !");
            noShootConfigLabel.AddToClassList("mb-8");
            noShootConfigBox.Add(noShootConfigLabel);

            noShootConfigBox.Add(new UnityEngine.UIElements.Label("Create a new one with name"));
            VisualElement horizontalBox = new VisualElement();
            horizontalBox.AddToClassList("align-horizontal");

            textField_DamageType.AddToClassList("flex-grow");

            createButton = new Button(() => CreateAbilityConfig(textField_DamageType.text, property));
            createButton.text = "Create";
            createButton.SetEnabled(newValue_ScriptableType != EScriptableType.NONE);
            textField_DamageType.SetEnabled(newValue_ScriptableType != EScriptableType.NONE);   

            horizontalBox.Add(textField_DamageType);
            horizontalBox.Add(createButton);

            noShootConfigBox.Add(horizontalBox);

            UnityEngine.UIElements.Label selectExistingLabel = new UnityEngine.UIElements.Label("Select Existing");
            selectExistingLabel.AddToClassList("bold");
            selectExistingLabel.AddToClassList("pt-4");
            selectExistingLabel.AddToClassList("mt-4");
            selectExistingLabel.AddToClassList("thin-border-top");
            noShootConfigBox.Add(selectExistingLabel);

            noShootConfigBox.Add(BuildObjectField(rootElement, property));

            return noShootConfigBox;
        }

        private VisualElement BuildAbilityConfigBox(VisualElement rootElement, SerializedProperty property)
        {
            EnumValueTracker.OnValueChanged_EScriptableType -= SetCreationText;

            VisualElement abilityConfigBox = new VisualElement();
            abilityConfigBox.name = "ability-config-box";

            abilityConfigBox.Add(BuildObjectField(rootElement, property));
            Button deleteButton = new Button(() => DeleteSO(property));
            deleteButton.text = "Delete";
            deleteButton.AddToClassList("danger");
            deleteButton.AddToClassList("align-right");
            deleteButton.AddToClassList("mb-8");
            abilityConfigBox.Add(deleteButton);

            SerializedObject abilityConfigSO = new SerializedObject(property.objectReferenceValue); // because we are dealing with scriptable objects

            UnityEngine.UIElements.Label damageBehavior = new UnityEngine.UIElements.Label("Damage/Enemy Interaction");
            damageBehavior.AddToClassList("bold");
            abilityConfigBox.Add(damageBehavior);

            helpBox_DamageType = new HelpBox("", HelpBoxMessageType.Info);
            enumField_DamageType.BindProperty(abilityConfigSO.FindProperty("DamageType"));
            helpBox_DamageType.text = hBTextFields[Convert.ToInt32(enumField_DamageType.value)];

            RegisterValueChanged_EDamageType(enumField_DamageType, helpBox_DamageType); 

            abilityConfigBox.Add(enumField_DamageType);
            abilityConfigBox.Add(helpBox_DamageType);

            VisualElement rangeContainer = new VisualElement();
            rangeContainer.AddToClassList("align-horizontal");

            Slider abilityCooldownSlider = new Slider("Delay Between Abilities", 0.001f, 3f);
            abilityCooldownSlider.BindProperty(abilityConfigSO.FindProperty("Cooldown"));
            abilityCooldownSlider.AddToClassList("flex-grow");
            FloatField abilityCooldownField = new FloatField();
            abilityCooldownField.style.minWidth = 35;
            abilityCooldownField.BindProperty(abilityConfigSO.FindProperty("Cooldown"));

            rangeContainer.Add(abilityCooldownSlider);
            rangeContainer.Add(abilityCooldownField);
            abilityConfigBox.Add(rangeContainer);

            return abilityConfigBox;
        }

        // BAD
        private void RegisterValueChanged_EDamageType<T>(T enumField, HelpBox helpBox) where T : EnumField
        {
            previousValue_DamageType = newValue_DamageType;

            enumField.RegisterValueChangedCallback((changeEvent) =>
            {
                EPayloadType newValue = (EPayloadType)changeEvent.newValue;
                switch (newValue)
                {
                    case EPayloadType.Burst:
                        {
                            helpBox.text = hBTextFields[0];
                            break;
                        }
                    case EPayloadType.OverTime:
                        {
                            helpBox.text = hBTextFields[1];
                            break;
                        }
                    case EPayloadType.Delayed:
                        {
                            helpBox.text = hBTextFields[2];
                            break;
                        }
                }

                newValue_DamageType = newValue;
            });
        }

        public void SetCreationText(EScriptableType newValue)
        {
            previousValue_ScriptableType = newValue; 

            if (newValue == EScriptableType.NONE)
            {
                createButton.SetEnabled(false);
                textField_DamageType.SetEnabled(false);
                textField_DamageType.value = string.Empty;
            }
            else
            {
                createButton.SetEnabled(true);
                textField_DamageType.SetEnabled(true); 
                textField_DamageType.value = $"{newValue} Config"; 
            }
        }

        /// <summary>
        /// Create a special field for a Scriptable Object
        /// </summary>
        /// <param name="rootElement"></param>
        /// <param name="property"></param>
        /// <returns></returns>
        private ObjectField BuildObjectField(VisualElement rootElement, SerializedProperty property)
        {
            ObjectField shootConfigObjectField = new ObjectField("Ability Config");
            shootConfigObjectField.objectType = typeof(AbilityConfigSO_Base);
            shootConfigObjectField.BindProperty(property.serializedObject.FindProperty("AbilityConfig"));

            AbilityConfigSO_Base currentValue = property.objectReferenceValue as AbilityConfigSO_Base;
            shootConfigObjectField.RegisterValueChangedCallback((changeEvent) =>
            {
                // if to avoid infinite looping by not checking against previous but current value
                if (changeEvent.newValue != currentValue)
                {
                    HandleChangeShootConfig(changeEvent, rootElement, property);
                }
            });

            return shootConfigObjectField;
        }

        private void HandleChangeShootConfig(ChangeEvent<Object> changeEvt, VisualElement root, SerializedProperty property)
        {
            root.Clear();
            BuildUI(root, property.serializedObject.FindProperty("AbilityConfig"));
        }

        private void CreateAbilityConfig(string Name, SerializedProperty property)
        {
            AbilityConfigSO_Base shootConfig = ScriptableObject.CreateInstance<AbilityConfigSO_Base>();
            AssetDatabase.CreateAsset(shootConfig, "Assets/Data/Gameplay/Combat/Abilities/" + Name + ".asset");
            AssetDatabase.SaveAssets(); 
            AssetDatabase.Refresh();    

            property.objectReferenceValue = shootConfig;
            property.serializedObject.ApplyModifiedProperties();
            property.serializedObject.Update();
        }

        private async void DeleteSO(SerializedProperty property)
        {
            string path = AssetDatabase.GetAssetPath(property.objectReferenceInstanceIDValue);
            property.objectReferenceValue = null;
            property.serializedObject.ApplyModifiedProperties();

            // to avoid rebuilding the whole drawer by doing a bad null check
            await Task.Delay(100);
            AssetDatabase.DeleteAsset(path);
        }

        private void HandleTitleClick(ClickEvent evt)
        {
            if (isExpanded)
            {
                CaretLabel.RemoveFromClassList("rotate-90");
                CaretLabel.AddToClassList("rotate-0");

                ContentPanel.RemoveFromClassList("expanded");
                ContentPanel.AddToClassList("collapsed");
            }
            else
            {
                CaretLabel.RemoveFromClassList("rotate-0");
                CaretLabel.AddToClassList("rotate-90");

                ContentPanel.RemoveFromClassList("collapsed");
                ContentPanel.AddToClassList("expanded");
            }

            isExpanded = !isExpanded;
        }
    }
}
