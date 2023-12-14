using System.Collections;
using System.Collections.Generic;

namespace UI
{
    using System;
    using Core.SingletonsSO;
    using DG.Tweening;
    using Player;
    using UnityEngine.SceneManagement;
    using UnityEngine.UIElements;
    using UnityEngine;

    public class MainMenuHandler : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDoc;
        [SerializeField] private DeviceLog devicelog;
        [SerializeField] private Texture2D[] riftAnimation;
        private VisualElement root;
        private VisualElement mainTitle;
        private VisualElement menu;
        private VisualElement mainMenu;
        private VisualElement optionMenu;
        private VisualElement sceneSelectMenu;
        private VisualElement keyArt;
        private VisualElement rift;
        
        private Button startButton;
        private Button optionButton;
        private Button returnFromOption;
        private Button protoXpButton;
        private Button protoGymButton;
        private Button returnFromSceneSelect;
        private Button exitButton;
        
        private RadioButtonGroup controlsRadioButtons;
        private Button newGame;
        private Label startLabel;

        private void Start()
        {
            root = uiDoc.rootVisualElement;
            startLabel = root.Q<Label>("StartLabel");
            
            mainTitle = root.Q<VisualElement>("MainTitle");
            keyArt = root.Q<VisualElement>("KeyArt");
            menu = root.Q<VisualElement>("Menu");
            mainMenu = root.Q<VisualElement>("MainMenu");
            optionMenu = root.Q<VisualElement>("OptionMenu");
            sceneSelectMenu = root.Q<VisualElement>("SceneSelectMenu");
            rift = root.Q<VisualElement>("Rift");
            
            startButton = root.Q<Button>("StartButton");
            optionButton = root.Q<Button>("Option");
            newGame = root.Q<Button>("NewGame");
            protoGymButton = root.Q<Button>("ProtoGym");
            protoXpButton = root.Q<Button>("ProtoXp");
            returnFromOption = root.Q<Button>("ReturnFromOption");
            returnFromSceneSelect = root.Q<Button>("SceneSelectBack");
            exitButton = root.Q<Button>("Exit");
            
            controlsRadioButtons = root.Q<RadioButtonGroup>("ControlsRadioButton");
            
            startButton.clicked += StartButtonPressed;
            newGame.clicked += NewGamePressed;
            optionButton.clicked += OptionMenuPressed;
            protoGymButton.clicked += () => LoadScene("ProtoGYM");
            protoXpButton.clicked += () => LoadScene("ProtoXP");
            returnFromOption.clicked += OptionBack;
            returnFromSceneSelect.clicked += SceneSelectReturn;
            exitButton.clicked += ReturnToDesktop;
            
            DOTween.To(() => startLabel.style.opacity.value, x => startLabel.style.opacity = x, 1, 1f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);
            UIHandler.Instance.ForceFocus(startButton);

        }
        

        private void StartButtonPressed()
        {
            startLabel.style.display = DisplayStyle.None;
            startButton.style.display = DisplayStyle.None;
            
            float finalPositionY = mainTitle.parent.layout.height * (73 / 100f);
            DOTween.To(() => mainTitle.layout.y, y => mainTitle.style.top = y, finalPositionY, 1f);
            
            DOTween.To(() => keyArt.style.opacity.value, x => keyArt.style.opacity = x, 1, 1f).SetEase(Ease.Linear).OnComplete(() =>
            {
                mainMenu.style.display = DisplayStyle.Flex;
                UIHandler.Instance.ForceFocus(newGame);
                StartCoroutine(AnimateImageSequence());
            });
        }
        
        private void OptionMenuPressed()
        {
            SquatchOnLeft(mainMenu, optionMenu);
            UIHandler.Instance.ForceFocus(controlsRadioButtons.ElementAt(0));
            Option();
            
        }
        private void Option()
        {
            optionMenu.style.visibility = Visibility.Visible;
            if (devicelog.currentDevice == Player.DeviceType.KEYBOARD)
            {
                controlsRadioButtons.value = 1;
            }
            else
            {
                controlsRadioButtons.value = 0;
            }
        }

        private void OptionBack()
        {
            SquatchOnLeft(optionMenu, mainMenu);
            UIHandler.Instance.ForceFocus(newGame);
        }

        private void SceneSelectReturn()
        {
            SquatchOnLeft(sceneSelectMenu,mainMenu);
            UIHandler.Instance.ForceFocus(newGame);
        }
        
        private void NewGamePressed()
        {
            SquatchOnLeft(mainMenu, sceneSelectMenu);
            UIHandler.Instance.ForceFocus(protoGymButton);
        }

        private void LoadScene(String sceneName)
        {
            SceneManager.LoadScene(sceneName);
        }

        private void SquatchOnLeft(VisualElement element, VisualElement elementToReturn)
        {
            DOTween.To(() => element.transform.scale.x,
                x => element.transform.scale = new Vector3(x, element.transform.scale.y, element.transform.scale.z), 0, 0.3f).OnComplete(() =>
                {
                    element.style.display = DisplayStyle.None;
                    SquatchReturn(elementToReturn);
                });
            
        }

        private void SquatchReturn(VisualElement element)
        {
            element.style.display = DisplayStyle.Flex;
            DOTween.To(() => element.transform.scale.x, x => element.transform.scale = new Vector3(x, element.transform.scale.y, element.transform.scale.z), 1, 0.3f);
        }
        
        private void ReturnToDesktop()
        {
            Application.Quit();
        }
        
        private IEnumerator AnimateImageSequence()
        {
            foreach (var image in riftAnimation)
            {
                rift.style.backgroundImage = new StyleBackground(image);
                yield return new WaitForSeconds(0.05f);
            }
            StartCoroutine(AnimateImageSequence());
        }

        
        
    }

}