using UnityEngine.Serialization;

namespace Core.Player
{
    using UnityEngine;
    using System.Collections;
    using UnityEngine.Animations.Rigging;

    public class AimGamepadPlayerState : PlayerState
    {
        private Vector2 aimDirection;
        private Coroutine aiming;
        [SerializeField] private PlayerStat playerStat;
        [SerializeField] private Rig aimRig;
        [SerializeField] private Transform aimHeadTarget;
        private Vector3 desiredAimTargetPos;
        public override void OnStateEnter()
        {
            aiming = StartCoroutine(Aim());
            playerStateInterpretor.weaponManager.AimWithCurrentWeapon();
            playerStateInterpretor.animator.SetBool("Aim", true);
            playerStat.isAiming = true;
            aimRig.weight = 1;

        }
        public override void Behave<T>(T x)
        {
            if (x is Vector2 aim)
                aimDirection = aim;
            if (aiming == default)
                aiming = StartCoroutine(Aim());
        }

        private IEnumerator Aim()
        {
            while (aimDirection.magnitude > 0.01f)
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
                playerStat.isAiming = false;
                aiming = default;
                playerStateInterpretor.weaponManager.RestWeapon();
                playerStateInterpretor.animator.SetBool("Aim", false);

            } 
        }
    }
}


