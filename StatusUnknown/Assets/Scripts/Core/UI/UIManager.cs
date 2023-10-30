namespace Core.UI
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance;

        [FoldoutGroup("Assets References"), Required]
        public UIInputsHandler inputsHandler;
        [FoldoutGroup("Scene References"), Required] 
        public UIDocument mainUI;
        
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
