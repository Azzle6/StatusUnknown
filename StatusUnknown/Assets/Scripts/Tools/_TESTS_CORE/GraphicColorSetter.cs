using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

public class GraphicColorSetter : MonoBehaviour
{
    [OnInspectorGUI(nameof(AdjustColor))]
    public ColorValueReference color = new ColorValueReference();
    private Graphic graphic;

    private void OnEnable()
    {
        AdjustColor();
    }

    private void AdjustColor()
    {
        if (graphic == null) graphic = GetComponent<Graphic>();

        if (graphic != null)
            graphic.color = color;  
    }
}
