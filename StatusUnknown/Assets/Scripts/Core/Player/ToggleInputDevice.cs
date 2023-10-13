using System.Collections;
using System.Collections.Generic;
using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using DeviceType = Core.Player.DeviceType;

public class ToggleInputDevice : MonoBehaviour
{
    public void SwitchInput()
    {
        if (DeviceLog.Instance.currentDevice == DeviceType.GAMEPAD)
        {
            DeviceLog.Instance.SetDevice(DeviceType.KEYBOARD);
            Debug.Log("Switched to Mouse");
        }
        else
        {
            DeviceLog.Instance.SetDevice(DeviceType.GAMEPAD);
            Debug.Log("Switched to Gamepad");
        }
    }
}
