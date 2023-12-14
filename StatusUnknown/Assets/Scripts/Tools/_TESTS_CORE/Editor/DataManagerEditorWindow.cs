using Sirenix.OdinInspector.Editor;
using UnityEditor;
using System.Linq;
using System;
using StatusUnknown.Tools;
using StatusUnknown.Content;
using Sirenix.OdinInspector;
using System.Dynamic;
using UnityEngine;
using Sirenix.Utilities.Editor;

namespace StatusUnknown.Utils.AssetManagement
{
    public class DataManagerEditorWindow : OdinMenuEditorWindow
    {
        private static Type[] typesToDisplay = TypeCache.GetTypesWithAttribute<ManageableDataAttribute>()
            .OrderBy(m => m.Name)
            .ToArray();

        private Type selectedType = typesToDisplay[0]; 

        [MenuItem(CoreToolsStrings.ROOT_MENU_PATH + "ScriptableObjects Manager")]
        private static void OpenEditor() => GetWindow<DataManagerEditorWindow>();

        protected override void OnGUI()
        {
            if (GUIUtils.UpdateDisplay_Click(ref selectedType, typesToDisplay))
                ForceMenuTreeRebuild();    

            base.OnGUI();
        }

        private CreateNewBurstConfig createNewData; 

        protected override OdinMenuTree BuildMenuTree()
        {
            createNewData = new CreateNewBurstConfig();

            OdinMenuTree tree = new OdinMenuTree
            {
                { "Create New", createNewData } // AS TEMPLATE (keep in mind : this only exists in memory on the heap, not as a unity file)
            };
            tree.AddAllAssetsAtPath(selectedType.Name, CoreAssetManagementStrings.PATH_ROOT_DATA, selectedType, true, true);

            return tree;
        }

        protected override void OnDestroy()
        {
            if (createNewData != null) 
            {
                DestroyImmediate(createNewData.burstConfig); 
                createNewData.Dispose();
            }
            base.OnDestroy();
        }

        protected override void OnBeginDrawEditors()
        {
            OdinMenuTreeSelection selected = MenuTree.Selection;
            SirenixEditorGUI.BeginHorizontalToolbar();

            {
                GUILayout.FlexibleSpace();

                // TODO : implement dynamic type casting
                if (SirenixEditorGUI.ToolbarButton("Delete Current Selection"))
                {
                    AbilityConfigSO_Burst asset = selected.SelectedValue as AbilityConfigSO_Burst;
                    string path = AssetDatabase.GetAssetPath(asset);
                    AssetDatabase.DeleteAsset(path);
                    AssetDatabase.SaveAssets(); 
                    AssetDatabase.Refresh();
                }
            }

            SirenixEditorGUI.EndHorizontalToolbar();
        }
    }

    public class CreateNewBurstConfig : IDisposable
    {
        // Track whether Dispose has been called.
        private bool _disposed = false;  

        // exists in memory, but not yet as a project file
        public CreateNewBurstConfig() 
        {
            burstConfig = ScriptableObject.CreateInstance<AbilityConfigSO_Burst>();
            burstConfig.name = "New Burst Config"; 
        }   

        [InlineEditor(InlineEditorObjectFieldModes.Hidden)]
        public AbilityConfigSO_Burst burstConfig;

        // this add it as a project file
        [Button("Add New Burst Config")]
        private void CreateNewData()
        {
            burstConfig = ScriptableObject.CreateInstance<AbilityConfigSO_Burst>();
            burstConfig.name = "New Burst Config";

            StatusUnknown_AssetManager.SaveSO(burstConfig, CoreAssetManagementStrings.PATH_ROOT_DATA, burstConfig.name, ".asset"); 
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
        }

        ~CreateNewBurstConfig()
        {
            Dispose(disposing: false);
        }
    }
}
