namespace Editor
{
    using Enemy;
    using UnityEditor;
    using UnityEngine;
    using static UnityEditor.PlayerSettings;

    [CustomEditor(typeof(EnemyContext), true)]
    public class EnemyStateEditor : Editor
    {
        const float yOffset = 1;
        private EnemyContext enemyContext;
        void OnSceneGUI()
        {
            if (!EditorApplication.isPlaying)
                return;

            enemyContext = (EnemyContext)target;
            Handles.Label(enemyContext.transform.position + Vector3.up * yOffset, $"State {enemyContext.stateName}");
        }
    }
}