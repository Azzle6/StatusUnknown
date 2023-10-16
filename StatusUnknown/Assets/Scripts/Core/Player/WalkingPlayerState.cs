namespace Core.Player
{
    using System.Collections;
    using UnityEngine;
    public class WalkingPlayerState : PlayerState
    {
        private Vector3 tempMovement;
        private float inertiaTimer;
        private Vector3 lookDirection;
        [SerializeField] private float groundCheckDistance = 0.5f;
        private Coroutine applyingMovement;
        private Coroutine applyingInertia;
        
        public override void OnStateEnter()
        {
            if ((inertiaTimer >0.1) && (applyingInertia != default))
                StopCoroutine(applyingInertia);
            applyingMovement = StartCoroutine(ApplyMovement());
        }

        public override void Behave<T>(T x)
        {
            if (x is Vector2 movement)
                Move(movement);
        }

        public override void OnStateExit()
        {
            applyingInertia = StartCoroutine(ApplyInertia());
            if (applyingMovement != default)
                StopCoroutine(applyingMovement);
            playerStateInterpretor.AddState("IdlePlayerState", PlayerStateType.MOVEMENT,false);
        }

        private void Move(Vector2 movement)
        {
            if (applyingInertia != default)
            {
                StopCoroutine(applyingInertia);
                applyingInertia = default;
            }
            if (applyingMovement == default)
                StartCoroutine(ApplyMovement());
            
            tempMovement.x = movement.x;
            tempMovement.z = movement.y;
        }

        private IEnumerator ApplyMovement()
        {
            while (tempMovement.magnitude > 0.01f)
            {
                playerStateInterpretor.rb.velocity = tempMovement * PlayerStat.Instance.moveSpeed + new Vector3(0,playerStateInterpretor.rb.velocity.y,0);
                if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null) 
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), tempMovement, PlayerStat.Instance.turnSpeed); 
                yield return null;
            }
        }


        IEnumerator ApplyInertia()
        {
            Vector3 initialVelocity = playerStateInterpretor.rb.velocity;
            tempMovement = Vector3.zero;
            inertiaTimer = 0;

            while (inertiaTimer < PlayerStat.Instance.inertiaDuration)
            {
                if (!CheckForGround())
                    inertiaTimer = PlayerStat.Instance.inertiaDuration;

                inertiaTimer += Time.deltaTime;
                playerStateInterpretor.rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, PlayerStat.Instance.inertiaCurve.Evaluate(inertiaTimer / PlayerStat.Instance.inertiaDuration));
                yield return null;
            }
        }

        private bool CheckForGround()
        {
            RaycastHit hit;
            if (Physics.Raycast(playerStateInterpretor.transform.position, Vector3.down, out hit, groundCheckDistance))
            {
                if (hit.collider != default)
                    return true;
            }
            return false;
        }
    }
}


