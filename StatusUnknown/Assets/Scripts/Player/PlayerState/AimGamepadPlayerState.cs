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
        
        public override void OnStateEnter()
        {
            /*if (aiming == default)
                aiming = StartCoroutine(Aim());*/
            Debug.Log("Entered aim state");
            playerStateInterpretor.weaponManager.AimWithCurrentWeapon();
            playerStateInterpretor.animator.SetBool("Aim", true);
            aimRig.weight = 1;
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
            yield return new WaitForSeconds(playerStat.timeBeforeStopAiming);
            if (!playerStat.isShooting && !playerStat.isAiming)
            {
                playerStateInterpretor.RemoveState(PlayerStateType.AIM);
            }
            Debug.Log("Relaunching coroutine");
            stopAiming = StartCoroutine(CheckIfStopAiming());
        }


       private IEnumerator Aim()
        {
            //match stick dead zone
            while (aimDirection.magnitude > 0.15f)
            {
                Debug.DrawRay(playerStateInterpretor.transform.position ,playerStateInterpretor.transform.forward *50, Color.blue);
                //Check angle of aim direction and change forward when angle is higher than limit 
                if (Vector3.Angle(playerStateInterpretor.transform.forward, new Vector3(aimDirection.x,0,aimDirection.y)) > playerStat.turnAngleLimit)
                {
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), new Vector3(aimDirection.x,0,aimDirection.y), playerStat.turnSpeed);
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
            if (aiming != default)
            {
                aimRig.weight = 0;
                StopCoroutine(aiming);
                aiming = default;
                playerStateInterpretor.weaponManager.RestWeapon();
                playerStateInterpretor.animator.SetBool("Aim", false);
            } 
        }
    }
}


