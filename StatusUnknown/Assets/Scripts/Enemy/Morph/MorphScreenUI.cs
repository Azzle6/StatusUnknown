

namespace Enemy.Morph
{
    using Unity.Mathematics;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class MorphScreenUI : MonoBehaviour
    {
        [SerializeField] UIDocument uiDocument;
        VisualElement indicatorElement;
       
        private void Awake()
        {
            indicatorElement = uiDocument.rootVisualElement.Q<VisualElement>("morphIndicator");
           
        }
        private void Update()
        {
            if (MorphEvents.activeMorphEgg != null)
            {
                var morphEgg = MorphEvents.activeMorphEgg;
                
                Vector3 screen = Camera.main.WorldToScreenPoint(morphEgg.transform.position);
                float posX = screen.x - indicatorElement.layout.width / 2;
                float posY = (Screen.height - screen.y) - indicatorElement.layout.height / 2;

                bool onScreen = posX > 0 && posY > 0 && posX < Screen.width && posY < Screen.height;
                indicatorElement.style.display = onScreen?DisplayStyle.None : DisplayStyle.Flex;

                posX = Mathf.Clamp(posX, 0,Screen.width - indicatorElement.layout.width);
                posY = Mathf.Clamp(posY, 0, Screen.height-indicatorElement.layout.height);

                indicatorElement.style.left = posX;
                indicatorElement.style.top = posY;
                indicatorElement.style.unityBackgroundImageTintColor = Color.Lerp(Color.white, Color.red, (Time.time - morphEgg.startMorphTime) / morphEgg.currentMorphDuration);
            }
            else
            {
                indicatorElement.style.display = DisplayStyle.None;
            }
        }
    }
}