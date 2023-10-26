using System;

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
        [SerializeField] private PlayerStat playerStat;

        
        private void Awake()
        {
            mainCamera = Camera.main;
        }
        
        public override void OnStateEnter()
        {
            aiming = StartCoroutine(Aim());
        }
        
        public override void Behave<T>(T x)
        {
            if (x is Vector2 aim)
                aimDirection = aim;
        }

        private IEnumerator Aim()
        {
            while (aimDirection.magnitude > 0.1f)
            {
                camToMouseRay = mainCamera.ScreenPointToRay(aimDirection);
                Debug.DrawRay(playerStateInterpretor.transform.position ,playerStateInterpretor.transform.forward *50, Color.blue);
                if (Physics.Raycast(camToMouseRay, out camToMouseHit, 100))
                {
                    if (camToMouseHit.collider != default)
                        yield return null;
                    mouseDirection = new Vector2(camToMouseHit.point.x - playerStateInterpretor.transform.position.x, camToMouseHit.point.z - playerStateInterpretor.transform.position.z);
                    playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), new Vector3(mouseDirection.x,0,mouseDirection.y), playerStat.turnSpeed);
                    //playerStateInterpretor.transform.LookAt(new Vector3(camToMouseHit.point.x,playerStateInterpretor.transform.position.y,camToMouseHit.point.z));
                }
                yield return null;
            }
        }
        

        public override void OnStateExit()
        {
            if (aiming != default)
                StopCoroutine(aiming);
        }

    
       
    }
    

}
