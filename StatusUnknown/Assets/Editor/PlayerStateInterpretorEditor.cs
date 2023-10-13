using System;

namespace Editor
{
    using UnityEngine;
    using UnityEditor;
    using Core.Player;
    
    [CustomEditor (typeof (PlayerStateInterpretor),true)]
    public class PlayerStateInterpretorEditor : Editor
    {
        private PlayerStateInterpretor psi;
        private GUIStyle style = new GUIStyle();

        private void Awake()
        {
            style.normal.textColor = Color.cyan;
            style.fontSize = 20;
        }

        void OnSceneGUI()
        {
            psi = (PlayerStateInterpretor)target;
            DisplayState();
        }

        public virtual void DisplayState()
        {
            if (EditorApplication.isPlaying)
            {         

                Handles.Label(psi.transform.position + Vector3.up * 2,
                    psi.statesSlot[PlayerStateType.MOVEMENT] != default
                        ? $"Movement : {psi.statesSlot[PlayerStateType.MOVEMENT].GetType().Name}"
                        : $"Movement : Null",style);
                
           
                Handles.Label(psi.transform.position + Vector3.up * 3,
                    psi.statesSlot[PlayerStateType.AIM] != default
                        ? $"Aim : {psi.statesSlot[PlayerStateType.AIM].GetType().Name}"
                        : $"Aim : Null", style);
                
             
                Handles.Label(psi.transform.position + Vector3.up * 4,
                    psi.statesSlot[PlayerStateType.ACTION] != default
                        ? $"Action : {psi.statesSlot[PlayerStateType.ACTION].GetType().Name}"
                        : $"Action : Null", style);
            }

        }

    }
}
