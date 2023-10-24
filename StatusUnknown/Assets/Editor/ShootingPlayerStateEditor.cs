namespace Editor
{
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
                
                if (shootingPlayerState.confirmedInTheFrustrum != null)
                {
                    Handles.color = Color.red;
            
                    for (int x = 0; x < shootingPlayerState.confirmedInTheFrustrum.Count; x++)
                    {
                        Collider collider = shootingPlayerState.confirmedInTheFrustrum[x];
                        float angle = shootingPlayerState.confirmedInTheAngle[x];
                        float angleRequired = shootingPlayerState.angleRequired[x];
                
                        if (angle <= angleRequired)
                        {
                            Vector3 targetPos = collider.transform.position;
                            Vector3 playerPos = shootingPlayerState.transform.parent.transform.position;
                            targetPos.y = 0;
                            playerPos.y = 0;

                            Handles.color = Color.green;
                            
            
                            Handles.DrawSolidArc(targetPos, Vector3.up, (playerPos- targetPos).normalized, angleRequired, 5f);
                            Handles.DrawSolidArc(targetPos, Vector3.up, (playerPos- targetPos).normalized, -angleRequired, 5f);
                        }
                    }
                }
            }
        }
    }
}

