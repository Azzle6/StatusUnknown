namespace Tools.PolybrushHelper.Editor
{
    using Sirenix.OdinInspector;
    using Sirenix.OdinInspector.Editor;
    using UnityEditor;
    using UnityEngine;

    public class PolybrushHelperWindow : OdinEditorWindow
    {
        [MenuItem("Status/PolybrushHelper")]
        private static void OpenWindow()
        {
            GetWindow<PolybrushHelperWindow>().Show();
        }
        
        [Button]
        public void ConvertSelectedMeshedToPolybrush()
        {
            Debug.Log(Selection.count);
            foreach (var obj in Selection.objects)
            {
                if (obj is GameObject go)
                {
                    MeshRenderer mesh = go.GetComponent<MeshRenderer>();
                    if (mesh != null)
                    {
                        //Do things here
                        //go.AddComponent<PolyBrushMesh>()
                    }
                }
            }
        }
    }
}
