using System;
using System.Collections;
using System.Collections.Generic;
using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DeviceType = Core.Player.DeviceType;

public class ToggleInputDevice : MonoBehaviour
{
    [SerializeField] private Text inputText;
    private void Awake()
    {
        inputText.text = DeviceLog.Instance.currentDevice.ToString();
    }

    public void SwitchInput()
    {
        if (DeviceLog.Instance.currentDevice == DeviceType.GAMEPAD)
        {
            DeviceLog.Instance.SetDevice(DeviceType.KEYBOARD);
            inputText.text = DeviceLog.Instance.currentDevice.ToString();
        }
        else
        {
            DeviceLog.Instance.SetDevice(DeviceType.GAMEPAD);
            inputText.text = DeviceLog.Instance.currentDevice.ToString();
        }
    }
}
