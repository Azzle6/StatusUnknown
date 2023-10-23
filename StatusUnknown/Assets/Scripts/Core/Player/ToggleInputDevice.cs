using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DeviceType = Core.Player.DeviceType;

public class ToggleInputDevice : MonoBehaviour
{
    [SerializeField] private Text inputText;
    [SerializeField] private DeviceLog deviceLog;
    private void Awake()
    {
        inputText.text = deviceLog.currentDevice.ToString();
    }

    public void SwitchInput()
    {
        if (deviceLog.currentDevice == DeviceType.GAMEPAD)
        {
            deviceLog.SetDevice(DeviceType.KEYBOARD);
            inputText.text = deviceLog.currentDevice.ToString();
        }
        else
        {
            deviceLog.SetDevice(DeviceType.GAMEPAD);
            inputText.text = deviceLog.currentDevice.ToString();
        }
    }
}
