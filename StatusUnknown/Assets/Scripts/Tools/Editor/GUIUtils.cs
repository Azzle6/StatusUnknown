using System;
using UnityEditor; 
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.Utilities;

public static class GUIUtils
{
    public static bool UpdateDisplay_Click(ref Type selectedType, Type[] typesToDisplay)
    {
        Rect rect = GUILayoutUtility.GetRect(0, 25); 

        for (int i = 0; i < typesToDisplay.Length; i++)
        {
            string name = typesToDisplay[i].Name;
            Rect btnRect = rect.Split(i, typesToDisplay.Length); 

            if (ButtonClicked(btnRect, name, typesToDisplay[i] == selectedType))
            {
                // updating what type should be displayed and fetched in tree.AddAllAssetsAtPath
                // redraw is done in DataManagerEditorWindow.ForceMenuTreeRebuild (OnGUI)
                selectedType = typesToDisplay[i]; 
                return true; 
            }
        }

        return false; 
    }

    public static bool ButtonClicked(Rect rect, string name, bool selected)
    {
        if (GUI.Button(rect, GUIContent.none, GUIStyle.none)) return true; 

        if (Event.current.type == EventType.Repaint)
        {
            GUIStyle style = new GUIStyle(EditorStyles.miniButtonMid); 
            style.stretchHeight = true;
            style.fixedHeight = rect.height;
            style.Draw(rect, GUIHelper.TempContent(name), false, false, selected, false); 
        }

        return false; 
    }

    public static bool UpdateDisplay_Hover(Rect rect, string name, bool hovered)
    {
        return false; 
    }
}
