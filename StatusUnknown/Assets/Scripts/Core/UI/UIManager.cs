namespace Core.UI
{
    using UnityEngine;
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        public UISettingsSO settings;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                if(Instance != this)
                    Debug.LogError("Multiple instance of UIManager exists.");
            }
        }
    }
}
