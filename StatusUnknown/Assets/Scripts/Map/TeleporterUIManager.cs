using Core;
using Interactable;
using Player;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Map
{
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class TeleporterUIManager : MonoSingleton<TeleporterUIManager>
    {
        [SerializeField] private EventSystem eventSystem;
        [SerializeField] private UIDocument mapsUIDoc;
        [SerializeField] private MapEncyclopedia mapEncyclopedia;
        private Dictionary<RadioButton, TeleporterMapData> radioButtonMapData = new Dictionary<RadioButton, TeleporterMapData>();
        private VisualElement mapDisplay;
        private VisualTreeAsset tempVisualTreeAsset;
        private VisualElement tempVisualElement;
        private RadioButtonGroup mapRadioButtonGroup;
        private string tempString;

        private void Awake()
        {
            mapDisplay = mapsUIDoc.rootVisualElement.Q<VisualElement>("MapDisplay");
            mapRadioButtonGroup = mapsUIDoc.rootVisualElement.Q<RadioButtonGroup>("RadioButtonMapGroup");
            mapsUIDoc.rootVisualElement.style.display = DisplayStyle.None;
        }
        public void Display()
        {
            mapsUIDoc.rootVisualElement.style.display = DisplayStyle.Flex;
            FetchCurrentMap();
            LoadRadioButton();
            mapRadioButtonGroup.RegisterCallback<ChangeEvent<int>>(OnRadioButtonChanged);
            eventSystem.SetSelectedGameObject(gameObject);
            PlayerInfoUIHandler.Instance.Hide();
        }

        private void LoadRadioButton()
        {
            mapRadioButtonGroup.Clear();
            tempString = SceneManager.GetActiveScene().name;
            int selectedIndex = -1;
            for (int i = 0; i < mapEncyclopedia.maps.Count; i++)
            {
                var mapData = mapEncyclopedia.maps[i];
                var radioButton = new RadioButton();
                radioButton.text = mapData.sceneName;
                mapRadioButtonGroup.Add(radioButton);
                radioButtonMapData[radioButton] = mapData;

                // Vérifiez si le nom de la scène de la carte correspond au nom de la scène actuelle
                if (mapData.sceneName == tempString)
                {
                    selectedIndex = i;
                }
            }

            // Si une correspondance a été trouvée, définissez le bouton radio sélectionné
            if (selectedIndex != -1)
            {
                mapRadioButtonGroup.SetValueWithoutNotify(selectedIndex);
            }
        }
        private void OnRadioButtonChanged(ChangeEvent<int> evt)
        {
            RadioButton selectedRadioButton = (RadioButton)mapRadioButtonGroup.ElementAt(evt.newValue);
            TeleporterMapData mapData = radioButtonMapData[selectedRadioButton];
            DisplayMap(mapData);
        }
        
        private void DisplayMap(TeleporterMapData mapData)
        {
            mapDisplay.Clear();
            tempVisualTreeAsset = mapData.sceneMap;
            tempVisualElement = tempVisualTreeAsset.CloneTree();
            mapDisplay.Add(tempVisualElement);
            tempVisualElement.style.width = Length.Percent(100);
            tempVisualElement.style.height = Length.Percent(100);
        }
        
        

        private void FetchCurrentMap()
        {
            tempString = SceneManager.GetActiveScene().name;
            if (mapEncyclopedia.tpMapData.ContainsKey(tempString))
            {
                tempVisualTreeAsset = mapEncyclopedia.tpMapData[tempString].sceneMap;
                tempVisualElement = tempVisualTreeAsset.CloneTree();
                mapDisplay.Add(tempVisualElement);
                tempVisualElement.style.width = Length.Percent(100);
                tempVisualElement.style.height = Length.Percent(100);
            }
            else
            {
                Debug.LogError("No map data found for the active scene.");
            }
        }
        
    }
}


