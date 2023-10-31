using System;
using UnityEngine.Serialization;

namespace Core.Player
{
    using UnityEngine;
    using System.Collections;
    
    public class AimMousePlayerState : PlayerState
    {
        private Camera mainCamera;
        private Vector2 aimDirection;
        private Vector2 mouseDirection;
        private Coroutine aiming;
        private Ray camToMouseRay;
        private RaycastHit camToMouseHit;
        private Vector3 desiredAimTargetPos;
        [SerializeField] private Transform aimHeadTarget;
        [SerializeField] private PlayerStat playerStat;

        
        private void Awake()
        {
            mainCamera = Camera.main;
        }
        
        public override void OnStateEnter()
        {
            aiming = StartCoroutine(Aim());
            playerStateInterpretor.weaponManager.AimWithCurrentWeapon();
            playerStateInterpretor.animator.SetBool("Aim", true);
            
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
            while (aimDirection.magnitude > 0.1f)
            {
                playerStat.isAiming = true;
                camToMouseRay = mainCamera.ScreenPointToRay(aimDirection);
                Debug.DrawRay(playerStateInterpretor.transform.position ,playerStateInterpretor.transform.forward *50, Color.blue);
                if (Physics.Raycast(camToMouseRay, out camToMouseHit, 100))
                {
                    playerStat.isAiming = true;
                    mouseDirection = new Vector2(camToMouseHit.point.x - playerStateInterpretor.transform.position.x, camToMouseHit.point.z - playerStateInterpretor.transform.position.z);
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), new Vector3(mouseDirection.x,0,mouseDirection.y), playerStat.turnSpeed);
                }
                else
                {
                    playerStat.isAiming = false;
                }

                yield return null;
            }
            playerStat.isAiming = false;
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
                StopCoroutine(aiming);
            playerStat.isAiming = false;
            playerStateInterpretor.weaponManager.RestWeapon();
            playerStateInterpretor.animator.SetBool("Aim", false);

        }
        
    }
    

}
