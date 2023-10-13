namespace Core.Player
{
    using Core.SingletonsSO;
    using UnityEngine;
    using UnityEngine.InputSystem;

    [CreateAssetMenu(fileName = "DeviceLog", menuName = "CustomAssets/DeviceLog", order = 1)]
    public class DeviceLog : SingletonSO<DeviceLog>
    {
        public InputDevice currentDevice;
        
        public void SetDevice(InputDevice device)
        {
            currentDevice = device;
        }
    }
}
