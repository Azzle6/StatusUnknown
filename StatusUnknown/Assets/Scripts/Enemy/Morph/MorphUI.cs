using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MorphUI : MonoBehaviour
{
    [SerializeField] MorphEgg morphEgg;
    [SerializeField] Image fillImage;


    private void Update()
    {
        fillImage.fillAmount = (Time.time - morphEgg.startMorphTime) / morphEgg.currentMorphDuration;
        fillImage.color = Color.Lerp(Color.white, Color.red, fillImage.fillAmount);
    }
}
