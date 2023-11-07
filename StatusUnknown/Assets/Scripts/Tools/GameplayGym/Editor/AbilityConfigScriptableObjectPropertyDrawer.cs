using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace StatusUnknown.CoreGameplayContent.Editors
{
    [CustomPropertyDrawer(typeof(AbilityConfigScriptableObject), true)]
    public class AbilityConfigScriptableObjectPropertyDrawer : PropertyDrawer
    {
        private VisualElement ContentPanel;
        private Label CaretLabel;
        private bool isExpanded = true;

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            VisualElement inspector = new VisualElement();
            inspector.AddToClassList("panel");

            return BuildUI(inspector, property);
        }

        private VisualElement BuildUI(VisualElement RootElement, SerializedProperty property)
        {
            VisualElement titleContainer = new VisualElement();
            titleContainer.AddToClassList("align-horizontal");
            CaretLabel = new Label(">");
            CaretLabel.style.fontSize = 18;
            CaretLabel.AddToClassList(isExpanded ? "rotate-90" : "rotate-0");
            titleContainer.Add(CaretLabel);
            Label title = new Label("Ability Configuration");
            titleContainer.AddToClassList("header");

            titleContainer.Add(title);
            titleContainer.RegisterCallback<ClickEvent>(HandleTitleClick);

            RootElement.Add(titleContainer);

            if (property.objectReferenceValue == null)
            {
                ContentPanel = BuildNotShootPanel(RootElement, property);
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

        private VisualElement BuildAbilityConfigBox(VisualElement rootElement, SerializedProperty property)
        {
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

            Label damageBehavior = new Label("Damage/Enemy Interaction");
            damageBehavior.AddToClassList("bold");
            abilityConfigBox.Add(damageBehavior);

            HelpBox dmgTypeHelpBox_DOT = new HelpBox("Damage Over Time", HelpBoxMessageType.Info);
            HelpBox dmgTypeHelpBox_Burst = new HelpBox("Instant application of damage", HelpBoxMessageType.Info);
            HelpBox dmgTypeHelpBox_Delayed = new HelpBox("Damage applied after a delay", HelpBoxMessageType.Info);

            EnumField damageTypeField = new EnumField("Damage Type", EDamageType.Burst);
            damageTypeField.BindProperty(abilityConfigSO.FindProperty("DamageType"));

            damageTypeField.RegisterValueChangedCallback((changeEvent) =>
            {
                EDamageType newValue = (EDamageType)changeEvent.newValue;

                switch (newValue)
                {
                    case EDamageType.Burst:
                        {
                            dmgTypeHelpBox_Burst.RemoveFromClassList("hidden");

                            dmgTypeHelpBox_DOT.AddToClassList("hidden");
                            dmgTypeHelpBox_Delayed.AddToClassList("hidden");
                            break;
                        }
                    case EDamageType.DOT:
                        {
                            dmgTypeHelpBox_DOT.RemoveFromClassList("hidden");

                            dmgTypeHelpBox_Burst.AddToClassList("hidden");
                            dmgTypeHelpBox_Delayed.AddToClassList("hidden");
                            break;
                        }
                    case EDamageType.Delayed:
                        {
                            dmgTypeHelpBox_Delayed.RemoveFromClassList("hidden");

                            dmgTypeHelpBox_DOT.AddToClassList("hidden");
                            dmgTypeHelpBox_Burst.AddToClassList("hidden");
                            break;
                        }
                }

            });
            abilityConfigBox.Add(damageTypeField);

            abilityConfigBox.Add(dmgTypeHelpBox_DOT);
            abilityConfigBox.Add(dmgTypeHelpBox_Burst);
            abilityConfigBox.Add(dmgTypeHelpBox_Delayed);

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

        private VisualElement BuildNotShootPanel(VisualElement rootElement, SerializedProperty property)
        {
            VisualElement noShootConfigBox = new VisualElement();
            noShootConfigBox.name = "no-ability-config-box";

            Label noShootConfigLabel = new Label("No Ability Config Exists !");
            noShootConfigLabel.AddToClassList("mb-8");
            noShootConfigBox.Add(noShootConfigLabel);

            noShootConfigBox.Add(new Label("Create a new one with name"));
            VisualElement horizontalBox = new VisualElement();
            horizontalBox.AddToClassList("align-horizontal");

            TextField soNameField = new();
            soNameField.AddToClassList("flex-grow");
            SerializedProperty abilityType = property.serializedObject.FindProperty("AbilityType"); 
            string abilityTypeName = abilityType.enumDisplayNames[abilityType.enumValueIndex];
            soNameField.value = $"{abilityTypeName} Config";

            Button createButton = new Button(() => CreateAbilityConfig(soNameField.text, property));
            createButton.text = "Create";
            createButton.SetEnabled(true);

            horizontalBox.Add(soNameField);
            horizontalBox.Add(createButton);

            noShootConfigBox.Add(horizontalBox);

            Label selectExistingLabel = new Label("Select Existing");
            selectExistingLabel.AddToClassList("bold");
            selectExistingLabel.AddToClassList("pt-4");
            selectExistingLabel.AddToClassList("mt-4");
            selectExistingLabel.AddToClassList("thin-border-top");
            noShootConfigBox.Add(selectExistingLabel);

            noShootConfigBox.Add(BuildObjectField(rootElement, property));

            return noShootConfigBox;
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
            shootConfigObjectField.objectType = typeof(AbilityConfigScriptableObject);
            shootConfigObjectField.BindProperty(property.serializedObject.FindProperty("AbilityConfig"));

            AbilityConfigScriptableObject currentValue = property.objectReferenceValue as AbilityConfigScriptableObject;
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

        // NOT WORKING - Because using AssetDataBase ?
        private void CreateAbilityConfig(string Name, SerializedProperty property)
        {
            AbilityConfigScriptableObject shootConfig = ScriptableObject.CreateInstance<AbilityConfigScriptableObject>();
            AssetDatabase.CreateAsset(shootConfig, Application.dataPath + "/Data/Gameplay/Combat/Abilities/" + Name + ".asset");
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
