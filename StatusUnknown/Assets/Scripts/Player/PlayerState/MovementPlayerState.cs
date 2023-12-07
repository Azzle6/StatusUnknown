namespace Player
{
    using System.Collections;
    using UnityEngine;
    public class MovementPlayerState : PlayerState
    {
        private Vector3 inputDirection;
        private Vector3 tempMovement;
        private Vector3 tempMovementAnim;
        private Vector3 initialVelocity;
        private Quaternion tempQuaternionAnim;
        private Vector3 camForward;
        private Vector3 camRight;
        private Vector3 forwardMovement;
        private Vector3 rightMovement;
        private float inertiaTimer;
        private Vector3 lookDirection;
        [SerializeField] private float groundCheckDistance = 0.5f;
        private Coroutine applyingMovement;
        private Coroutine applyingInertia;
        private RaycastHit groundHit;
        private RaycastHit slopeHit;
        private bool slopeDetected;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private DeviceLog deviceLog;
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
            playerStateInterpretor.animator.SetBool("Walk", true);

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
            playerStateInterpretor.animator.SetFloat("WalkMagnitude", 0);
            playerStateInterpretor.animator.SetBool("Walk", false);
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
            
            inputDirection = new Vector3(movement.x, 0, movement.y);
            tempMovementAnim = inputDirection;
            
            AdjustAnimAcordingToAim();
        }

        private IEnumerator ApplyMovement()
        {
            while (inputDirection.magnitude > 0.01f)
            {
                //si la camera ne se déplace pas frequement déplacer cela dans l'init ? 
                camForward = cam.transform.forward; 
                camRight = cam.transform.right;
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();
                
                forwardMovement = camForward * inputDirection.z;
                rightMovement = camRight * inputDirection.x;

                if (playerStat.isShooting)
                    tempMovement = (forwardMovement + rightMovement) * playerStat.moveSpeedWhileShooting;
                else
                    tempMovement = (forwardMovement + rightMovement) * playerStat.moveSpeed;

                if (playerStat.isAiming == default)
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), tempMovement.normalized, playerStat.turnSpeed);

                //diagonal danger
                if (tempMovement.magnitude > playerStat.moveSpeed)
                    tempMovement = tempMovement.normalized * playerStat.moveSpeed;
                
                tempMovement.y = playerStateInterpretor.rb.velocity.y;
                playerStateInterpretor.rb.velocity = tempMovement;
                AdjustVelocityToSlope();
                yield return null;
            }
        }


        IEnumerator ApplyInertia()
        {
            initialVelocity = playerStateInterpretor.rb.velocity;
            tempMovement = Vector3.zero;
            inputDirection = Vector3.zero;
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

        private void AdjustAnimAcordingToAim()
        {
            tempMovementAnim = playerStateInterpretor.transform.InverseTransformDirection(tempMovement); ;
            playerStateInterpretor.animator.SetFloat("WalkMagnitude", inputDirection.magnitude);
            playerStateInterpretor.animator.SetFloat("WalkDirX", tempMovementAnim.x);
            playerStateInterpretor.animator.SetFloat("WalkDirY", tempMovementAnim.z);
        }

        private void AdjustVelocityToSlope()
        {
            if (Physics.Raycast(playerStateInterpretor.transform.position, Vector3.down, out slopeHit, groundCheckDistance))
            {
                if (slopeHit.collider == default)
                    return;
                
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


