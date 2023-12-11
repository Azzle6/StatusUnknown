using Core.SingletonsSO;
using Player;
using UnityEngine.SceneManagement;

namespace UI
{
    using UnityEngine;
    using Core.EventsSO.GameEventsTypes;
    using UnityEngine.UIElements;

    public class PauseMenuUIHandler : MonoBehaviour
    {
        [SerializeField] private UIDocument uiDocument;
        [SerializeField] private BoolGameEvent playerInfo;
        [SerializeField] private DeviceLog deviceLog;
        private Button resumeButton;
        private Button restartButton;
        private Button optionButton;
        private Button exitButton;
        private Button optionReturnButton;
        private RadioButtonGroup controlRadioButtons;
        private VisualElement pauseMenu;
        private VisualElement optionMenu;

        private void Start()
        {
            uiDocument.rootVisualElement.style.display = DisplayStyle.None;
            resumeButton = uiDocument.rootVisualElement.Q<Button>("ResumeButton");
            restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
            optionButton = uiDocument.rootVisualElement.Q<Button>("OptionButton");
            exitButton = uiDocument.rootVisualElement.Q<Button>("ExitButton");
            pauseMenu = uiDocument.rootVisualElement.Q<VisualElement>("PauseMenu");
            optionMenu = uiDocument.rootVisualElement.Q<VisualElement>("OptionMenu");
            controlRadioButtons = uiDocument.rootVisualElement.Q<RadioButtonGroup>("ControlRadioButtons");
            optionReturnButton = uiDocument.rootVisualElement.Q<Button>("OptionReturnButton");
            
            resumeButton.RegisterCallback<ClickEvent>(e => Resume());
            restartButton.RegisterCallback<ClickEvent>(e => Restart());
            optionButton.RegisterCallback<ClickEvent>(e => Option());
            exitButton.RegisterCallback<ClickEvent>(e => Exit());
            optionReturnButton.RegisterCallback<ClickEvent>(e => OptionBack());
        }

        private void SwitchDisplay()
        {
            uiDocument.rootVisualElement.style.display = uiDocument.rootVisualElement.style.display == DisplayStyle.None ? DisplayStyle.Flex : DisplayStyle.None;
            playerInfo.RaiseEvent(uiDocument.rootVisualElement.style.display == DisplayStyle.Flex);

            if (uiDocument.rootVisualElement.style.display == DisplayStyle.Flex)
            {
                Time.timeScale = 0;
                optionMenu.style.visibility = Visibility.Hidden;
                pauseMenu.style.visibility = Visibility.Visible;
                UIHandler.Instance.ForceFocus(resumeButton);
            }
            else
            {
                Time.timeScale = 1;
                if (controlRadioButtons.value == 0)
                    deviceLog.currentDevice = Player.DeviceType.GAMEPAD;
                if (controlRadioButtons.value == 1) 
                    deviceLog.currentDevice = Player.DeviceType.KEYBOARD;
            }
        }

        private void Resume()
        {
            Time.timeScale = 1;
            SwitchDisplay();
        }
        
        private void Restart()
        {
            Time.timeScale = 1;
            UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void Option()
        {
            pauseMenu.style.visibility = Visibility.Hidden;
            optionMenu.style.visibility = Visibility.Visible;
            if (deviceLog.currentDevice == Player.DeviceType.KEYBOARD)
            {
                controlRadioButtons.value = 1;
            }
            else
            {
                controlRadioButtons.value = 0;
            }
        }
        
        private void OptionBack()
        {
            pauseMenu.style.visibility = Visibility.Visible;
            optionMenu.style.visibility = Visibility.Hidden;
        }
        
        private void Exit()
        {
            SceneManager.LoadScene("MainMenu");
        }


        public void ReceiveSignal()
        {
            SwitchDisplay();
        }
    }

}