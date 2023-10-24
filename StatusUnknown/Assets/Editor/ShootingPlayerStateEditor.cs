namespace Editor
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEditor;
    using Core.Player;
    
    
    [CustomEditor(typeof(ShootingPlayerState), true)]
    public class ShootingPlayerStateEditor : Editor
    {
        private ShootingPlayerState shootingPlayerState;
        private GUIStyle style = new GUIStyle();

        private void Awake()
        {
            style.normal.textColor = Color.cyan;
            style.fontSize = 20;
        }
        private void OnSceneGUI()
        {
            shootingPlayerState = (ShootingPlayerState)target;
            DisplayTargetAngle();
        }

        protected virtual void DisplayTargetAngle()
        {
            if (EditorApplication.isPlaying)
            {
                if (shootingPlayerState.confirmedInTheFrustrum.Count > 0)
                {
                    for (int x = 0; x < shootingPlayerState.confirmedInTheFrustrum.Count; x++)
                    {
                        Handles.Label(shootingPlayerState.confirmedInTheFrustrum[x].transform.position + Vector3.up * 3,
                            $"Angle : {shootingPlayerState.confirmedInTheAngle[x]}", style);
                        Handles.Label(shootingPlayerState.confirmedInTheFrustrum[x].transform.position + Vector3.up * 4,
                            $"Angle Required : {shootingPlayerState.angleRequired[x]}", style);
                    }
                }

                if (shootingPlayerState.closestTarget != default)
                {
                    Handles.Label(shootingPlayerState.closestTarget.transform.position + Vector3.up * 5, "I am the target", style);
                }
            }
        }
    }

}

