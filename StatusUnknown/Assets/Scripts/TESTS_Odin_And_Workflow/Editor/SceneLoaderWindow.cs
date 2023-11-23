using Sirenix.OdinInspector.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.SceneManagement;
using StatusUnknown.Tools;

public class SceneLoaderWindow : OdinEditorWindow
{
    [MenuItem(SU_Tools.ROOT_MENU_PATH + "Scene Loader")]
    // Start is called before the first frame update
    void OpenSceneLoader()
    {
        var window = GetWindow<SceneLoaderWindow>();
        window.Show();         
    }

    [ButtonGroup]
    private void StartScene()
    {
        LoadScene("Assets/Scenes/MyScene.unity");
    }

    [ButtonGroup]
    private void LoadProjectNotes()
    {
        Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/Prefabs/Status Unknown - General.asset"); // scriptable object
    }

    private void LoadScene(string scenePath)
    {
        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
            EditorSceneManager.OpenScene(scenePath);
    }
}
