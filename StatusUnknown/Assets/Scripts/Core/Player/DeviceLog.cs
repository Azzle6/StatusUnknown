namespace Core.Player
{
    using Core.SingletonsSO;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(fileName = "DeviceLog", menuName = "CustomAssets/DeviceLog", order = 1)]
    public class DeviceLog : ScriptableObject
    {
        public DeviceType currentDevice = DeviceType.KEYBOARD;
        
        public void SetDevice(DeviceType device)
        {
            currentDevice = device;
        }
    }
    
    public enum DeviceType
    {
        KEYBOARD,
        GAMEPAD
    }
}
