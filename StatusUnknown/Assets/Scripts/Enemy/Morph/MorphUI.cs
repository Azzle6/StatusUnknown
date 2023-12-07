using UnityEngine.UI;
using UnityEngine;
using System.Collections;

public class MorphUI : MonoBehaviour
{
    [SerializeField] MorphEgg morphEgg;
    [SerializeField] Image fillImage;
    float startTime;

    private void Start()
    {
        startTime = Time.time; 
    }
    private void Update()
    {
        fillImage.fillAmount = (Time.time - startTime) / morphEgg.currentMorphDuration;
        fillImage.color = Color.Lerp(Color.white, Color.red, fillImage.fillAmount);
    }
}
