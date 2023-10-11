namespace Core.Player
{
    using System.Collections;
    using UnityEngine;
    public class WalkingPlayerState : PlayerState
    {
        private Vector3 tempMovement;
        [SerializeField] private float moveSpeed;
        [SerializeField] private AnimationCurve inertiaCurve;
        [SerializeField] private float inertiaDuration;
        [SerializeField] private float turnSpeed = 0.5f;
        private float inertiaTimer;
        private bool applyingInertia;
        private Vector3 lookDirection;
        public override void OnStateEnter()
        {
            if (applyingInertia)
            {
                StopAllCoroutines();
                applyingInertia = false;
            }
            StartCoroutine(ApplyMovement());
        }

        public override void Behave(Vector2 movement)
        {
            Move(movement);
        }

        public override void OnStateExit()
        {
            StartCoroutine(ApplyInertia());
            playerStateInterpretor.AddState("IdlePlayerState", PlayerStateType.MOVEMENT);
        }

        private void Move(Vector2 movement)
        {
            if (movement.magnitude < 0.1f)
                playerStateInterpretor.RemoveState(PlayerStateType.MOVEMENT);
            
            tempMovement.x = movement.x;
            tempMovement.z = movement.y;
        }

        private IEnumerator ApplyMovement()
        {
            yield return new WaitForSeconds(0.05f);
            while (tempMovement.magnitude > 0.01f)
            {
                playerStateInterpretor.rb.velocity = (tempMovement + new Vector3(0,playerStateInterpretor.rb.velocity.y,0)) * moveSpeed;
                if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null) 
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), tempMovement, turnSpeed); 
                yield return null;
            }
        }


        IEnumerator ApplyInertia()
        {
            applyingInertia = true;
            Vector3 initialVelocity = playerStateInterpretor.rb.velocity;
            tempMovement = Vector3.zero;
            inertiaTimer = 0;

            while (inertiaTimer < inertiaDuration)
            {
                inertiaTimer += Time.deltaTime;
                playerStateInterpretor.rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, inertiaCurve.Evaluate(inertiaTimer / inertiaDuration));
                yield return null;
            }
            playerStateInterpretor.rb.velocity = Vector3.zero;
            applyingInertia = false;
        }
    }
}


