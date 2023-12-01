namespace Editor
{
    using Player;
    using UnityEditor;
    using UnityEngine;

    [CustomEditor(typeof(MeleeActivePlayerState), true)]
    public class MeleeActivePlayerStateEditor : Editor
    {
        private MeleeActivePlayerState meleeActivePlayerState;

        private void OnSceneGUI()
        {
            meleeActivePlayerState = (MeleeActivePlayerState)target;
            DisplayAttackAngle();
        }
        private void DisplayAttackAngle()
        {
            if (meleeActivePlayerState.currentAttack != null)
            {
                Handles.color = new Color(0, 0, 1, 0.5f);
                Vector3 attackDirection = meleeActivePlayerState.transform.forward;
                float attackAngle = meleeActivePlayerState.currentAttack.attackAngle;
                float attackRange = meleeActivePlayerState.currentAttack.attackLength;
                Handles.DrawSolidArc(meleeActivePlayerState.transform.position, Vector3.up, Quaternion.Euler(0, -attackAngle / 2, 0) * attackDirection, attackAngle, attackRange);
            }
        }

    }
}

