using System.Collections;
using System.Collections.Generic;
using Core.Player;
using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleInputDevice : MonoBehaviour
{
    public void SwitchInput()
    {
        if (DeviceLog.Instance.currentDevice == Mouse.current)
        {
            DeviceLog.Instance.SetDevice(Gamepad.current);
            Debug.Log("Switched to Gamepad");
        }
        else
        {
            DeviceLog.Instance.SetDevice(Mouse.current);
            Debug.Log("Switched to Mouse");
        }
    }
}
