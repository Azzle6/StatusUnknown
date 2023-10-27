namespace Core.Player
{
    using System.Collections;
    using UnityEngine;
    public class WalkingPlayerState : PlayerState
    {
        private Vector3 tempMovement;
        private Vector3 tempMovementAnim;
        private Quaternion tempQuaternionAnim;
        private float inertiaTimer;
        private Vector3 lookDirection;
        [SerializeField] private float groundCheckDistance = 0.5f;
        private Coroutine applyingMovement;
        private Coroutine applyingInertia;
        private RaycastHit groundHit;
        private RaycastHit slopeHit;
        private bool slopeDetected;
        [SerializeField] private PlayerStat playerStat;
        private Camera cam;
        
        private void Awake()
        {
            cam = Camera.main;
        }
        
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
            playerStateInterpretor.animator.SetFloat("WalkDirX", 0);
            playerStateInterpretor.animator.SetFloat("WalkDirY", 0);
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
            
            tempMovement = new Vector3(movement.x,0,movement.y);
            tempMovementAnim = tempMovement;
            tempMovement = cam.transform.TransformDirection(tempMovement);
            tempMovementAnim = tempMovement; 
            tempMovement.y = 0;
            
            tempQuaternionAnim = Quaternion.FromToRotation(Vector3.forward, playerStateInterpretor.transform.forward);
            tempQuaternionAnim = Quaternion.Inverse(tempQuaternionAnim);
            tempMovementAnim = tempQuaternionAnim * tempMovement;
            
            playerStateInterpretor.animator.SetFloat("WalkDirX", tempMovementAnim.x);
            playerStateInterpretor.animator.SetFloat("WalkDirY", tempMovementAnim.z);
        }

        private IEnumerator ApplyMovement()
        {
            while (tempMovement.magnitude > 0.01f)
            {
                playerStateInterpretor.rb.velocity = tempMovement * playerStat.moveSpeed + new Vector3(0,playerStateInterpretor.rb.velocity.y,0);
                AdjustVelocityToSlope();
                if (playerStat.isAiming == default) 
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), tempMovement, playerStat.turnSpeed); 
                yield return null;
            }
        }


        IEnumerator ApplyInertia()
        {
            Vector3 initialVelocity = playerStateInterpretor.rb.velocity;
            tempMovement = Vector3.zero;
            inertiaTimer = 0;

            while (inertiaTimer < playerStat.inertiaDuration)
            {
                if (!CheckForGround())
                    inertiaTimer = playerStat.inertiaDuration;

                inertiaTimer += Time.deltaTime;
                playerStateInterpretor.rb.velocity = Vector3.Lerp(initialVelocity, Vector3.zero, playerStat.inertiaCurve.Evaluate(inertiaTimer / playerStat.inertiaDuration));
                AdjustVelocityToSlope();
                yield return null;
            }
        }

        private void AdjustVelocityToSlope()
        {
            if (Physics.Raycast(playerStateInterpretor.transform.position, Vector3.down, out slopeHit, groundCheckDistance))
            {
                if (slopeHit.collider != default)
                {
                    if (slopeHit.normal != Vector3.up)
                    {
                        playerStateInterpretor.rb.velocity = Vector3.ProjectOnPlane(playerStateInterpretor.rb.velocity, slopeHit.normal);
                        slopeDetected = true;
                    }

                    if ((slopeHit.normal == Vector3.up) && (slopeDetected))
                    {
                        playerStateInterpretor.rb.velocity = Vector3.zero;
                        slopeDetected = false;
                    }
                }
            }
        }
        
        
        

        private bool CheckForGround()
        {
            if (Physics.Raycast(playerStateInterpretor.transform.position, Vector3.down, out groundHit, groundCheckDistance))
            {
                if (groundHit.collider != default)
                    return true;
            }
            return false;
        }
    }
}


