

namespace Map
{
using System.Collections.Generic;
using UnityEngine;
using Core;
using Core.SingletonsSO;
using Interactable;
using Player;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

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
        private List<Button> currentMapButtons = new List<Button>();
        private Teleporter currentTeleporter;

        private void Awake()
        {
            mapDisplay = mapsUIDoc.rootVisualElement.Q<VisualElement>("MapDisplay");
            mapRadioButtonGroup = mapsUIDoc.rootVisualElement.Q<RadioButtonGroup>("RadioButtonMapGroup");
            mapsUIDoc.rootVisualElement.style.display = DisplayStyle.None;
        }
        public void Display(Teleporter teleporter)
        {
            currentTeleporter = teleporter;
            mapsUIDoc.rootVisualElement.style.display = DisplayStyle.Flex;
            PlayerAction.Instance.DisableEvent();
            FetchCurrentMap();
            LoadRadioButton();
            mapRadioButtonGroup.RegisterCallback<ChangeEvent<int>>(OnRadioButtonChanged);
            PlayerInfoUIHandler.Instance.Hide();
        }

        private void Hide()
        {
            mapRadioButtonGroup.UnregisterCallback<ChangeEvent<int>>(OnRadioButtonChanged);
            mapsUIDoc.rootVisualElement.style.display = DisplayStyle.None;

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
                radioButton.focusable = true;
                radioButtonMapData[radioButton] = mapData;
                
                if (mapData.sceneName == tempString)
                {
                    selectedIndex = i;
                }
            }
            
            if (selectedIndex != -1)
            {
                mapRadioButtonGroup.SetValueWithoutNotify(selectedIndex);
                UIHandler.Instance.ForceFocus(mapRadioButtonGroup.ElementAt(0));
            }
        }

        private void LoadMapButton()
        {
            currentMapButtons.Clear();
            VisualElement map = tempVisualElement.Q<VisualElement>("Map");
            for (int x = 0; x < map.childCount; x++)
            {
                map.ElementAt(x).Q<Button>().RegisterCallback<ClickEvent>(OnTpButton);
            }

        }
        private void OnTpButton(ClickEvent clickEvent)
        {
            Button clickedButton = (Button)clickEvent.currentTarget;
            VisualElement parent = clickedButton.parent;
            int index = parent.IndexOf(clickedButton);
            currentTeleporter.ReceiveTeleporterData(mapEncyclopedia.tpMapData[tempString].teleporterData[index]);
            mapDisplay.Clear();
            mapRadioButtonGroup.Clear();

            Hide();
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
            LoadMapButton();
            tempString = mapData.sceneName;
            tempVisualElement.style.width = Length.Percent(100);
            tempVisualElement.style.height = Length.Percent(100);
        }
        
        

        private void FetchCurrentMap()
        {
            tempString = SceneManager.GetActiveScene().name;
            mapDisplay.Clear();
            if (mapEncyclopedia.tpMapData.ContainsKey(tempString))
            {
                tempVisualTreeAsset = mapEncyclopedia.tpMapData[tempString].sceneMap;
                tempVisualElement = tempVisualTreeAsset.CloneTree();
                mapDisplay.Add(tempVisualElement);
                LoadMapButton();
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


