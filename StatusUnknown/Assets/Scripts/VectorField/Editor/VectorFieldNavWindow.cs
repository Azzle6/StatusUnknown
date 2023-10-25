using UnityEditor;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UIElements;

public class VectorFieldNavWindow : EditorWindow
{
    [SerializeField]
    VisualTreeAsset tree;

    private Slider fieldDensity;
    private Slider maxAngle;
    private FloatField maxStep;
    private Button bakeButton;
    

    [MenuItem("Tools/VectorFieldNavigation")]
    public static void ShowEditor()
    {
        var window = GetWindow<VectorFieldNavWindow>();
        window.titleContent = new GUIContent("VectorFieldNavigation");
    }

    private void CreateGUI()
    {
        tree.CloneTree(rootVisualElement);
        InitFields();
    }

    private void InitFields()
    {
        fieldDensity = rootVisualElement.Q<Slider>("FieldDensity");
        fieldDensity.value = VectorFieldNavigator.fieldDensity;
        fieldDensity.RegisterValueChangedCallback(evt =>
        {
            VectorFieldNavigator.fieldDensity = evt.newValue;
        });
        maxAngle = rootVisualElement.Q<Slider>("MaxAngle");
        maxStep = rootVisualElement.Q<FloatField>("MaxAngle");
        bakeButton = rootVisualElement.Q<Button>("Bake");
        bakeButton.clickable.clicked += delegate { VectorFieldNavigator.BakeAllActiveVolume(); };
    }
}


