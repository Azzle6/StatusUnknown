namespace Player
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.Animations.Rigging;

    public class AimGamepadPlayerState : PlayerState
    {
        private Vector2 aimDirection;
        private Coroutine aiming;
        private Coroutine stopAiming;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private Rig aimRig;
        [SerializeField] private Transform aimHeadTarget;
        private Vector3 desiredAimTargetPos;
        private Vector3 camForward;
        private Vector3 camRight;
        private Vector3 finalAimDirection;
        private Camera cam;
        
        private void Awake()
        {
            cam = Camera.main;
        }
        
        public override void OnStateEnter()
        {
            if (aiming == default)
                aiming = StartCoroutine(Aim());
            if (!playerStat.currentWeaponIsMelee)
            {
                playerStateInterpretor.weaponManager.AimWithCurrentWeapon();
                aimRig.weight = 1;
                playerStateInterpretor.animator.SetBool("Aim", true);
            }
            
            
            if (stopAiming == default)
                stopAiming = StartCoroutine(CheckIfStopAiming());

        }
        public override void Behave<T>(T x)
        {
            if (x is Vector2 aim)
            {
                aimDirection = aim;
                Aim();
            }
            aiming ??= StartCoroutine(Aim());
        }
        
        private IEnumerator CheckIfStopAiming()
        {
            while (true)
            {
                yield return new WaitForSeconds(playerStat.timeBeforeStopAiming);
                if (!playerStat.isShooting && !playerStat.isAiming)
                {
                    playerStateInterpretor.RemoveState(PlayerStateType.AIM);
                    yield break;
                }
            }
        }


       private IEnumerator Aim()
        {
            //match stick dead zone
            while (playerStat.isAiming)
            {
                Debug.DrawRay(playerStateInterpretor.transform.position ,playerStateInterpretor.transform.forward *50, Color.blue);
                camForward = cam.transform.forward; 
                camRight = cam.transform.right;
                camForward.y = 0;
                camRight.y = 0;
                camForward.Normalize();
                camRight.Normalize();
                
                finalAimDirection = camForward * aimDirection.y + camRight * aimDirection.x;
                
                //Check angle of aim direction and change forward when angle is higher than limit 
                if (Vector3.Angle(playerStateInterpretor.transform.forward, finalAimDirection) > playerStat.turnAngleLimit)
                {
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), finalAimDirection, playerStat.turnSpeed);
                }
                HeadRotation();
                yield return null;
            }
        }

        private void HeadRotation()
        {
            desiredAimTargetPos = new Vector3(aimDirection.x,0,aimDirection.y);
            desiredAimTargetPos = desiredAimTargetPos.normalized * 3;
            desiredAimTargetPos.y = playerStat.headHeightOffset;
            aimHeadTarget.position = playerStateInterpretor.transform.position + desiredAimTargetPos;
        }

       

        public override void OnStateExit()
        {
                aimRig.weight = 0;
                if (aiming != default)
                    StopCoroutine(aiming);
                if (stopAiming != default)
                    StopCoroutine(stopAiming);
                stopAiming = default;
                aiming = default;
                playerStat.isAiming = false;
                playerStateInterpretor.weaponManager.RestWeapon();
                playerStateInterpretor.animator.SetBool("Aim", false);
        }
    }
}


