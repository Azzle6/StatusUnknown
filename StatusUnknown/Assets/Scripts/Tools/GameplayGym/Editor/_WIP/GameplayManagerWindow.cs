using System.Linq;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class GameplayManagerWindow : EditorWindow
{
    /* [SerializeField] private GameplayManager _GameplayManager;

    [MenuItem("Tool/Status Unknown/Gameplay Manager")]
    static void CreateMenu()
    {
        var window = GetWindow<GameplayManagerWindow>();
        window.titleContent = new GUIContent("Complex"); 
    }

    public void OnEnable()
    {
        _GameplayManager = GameObject.FindGameObjectsWithTag("Camera").FirstOrDefault()?.GetComponent<GameplayManager>();   
    }

    public void CreateGUI()
    {
        if (_GameplayManager == null) return;

        var scrollView = new ScrollView() { viewDataKey = "WindowScrollView" };
        scrollView.Add(new InspectorElement(_GameplayManager)); 
        rootVisualElement.Add(scrollView);  
    } */
}
