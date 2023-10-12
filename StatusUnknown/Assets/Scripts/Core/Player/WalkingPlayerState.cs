namespace Core.Player
{
    using System.Collections;
    using UnityEngine;
    public class WalkingPlayerState : PlayerState
    {
        private Vector3 tempMovement;
        private float inertiaTimer;
        private bool applyingInertia;
        private Vector3 lookDirection;
        private Vector3 targetInertia;
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
            playerStateInterpretor.AddState("IdlePlayerState", PlayerStateType.MOVEMENT,false);
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
                playerStateInterpretor.rb.velocity = (tempMovement + new Vector3(0,playerStateInterpretor.rb.velocity.y*Time.deltaTime,0)) * PlayerStat.Instance.moveSpeed;
                if (playerStateInterpretor.statesSlot[PlayerStateType.AIM] == null) 
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), tempMovement, PlayerStat.Instance.turnSpeed); 
                yield return null;
            }
        }


        IEnumerator ApplyInertia()
        {
            applyingInertia = true;
            Vector3 initialVelocity = playerStateInterpretor.rb.velocity;
            tempMovement = Vector3.zero;
            inertiaTimer = 0;

            while (inertiaTimer < PlayerStat.Instance.inertiaDuration)
            {
                inertiaTimer += Time.deltaTime;
                targetInertia = Vector3.zero + new Vector3(0,playerStateInterpretor.rb.velocity.y,0);
                playerStateInterpretor.rb.velocity = Vector3.Lerp(initialVelocity, targetInertia, PlayerStat.Instance.inertiaCurve.Evaluate(inertiaTimer / PlayerStat.Instance.inertiaDuration));
                yield return null;
            }
            playerStateInterpretor.rb.velocity = Vector3.zero;
            applyingInertia = false;
        }
    }
}


