
using System.Collections;
using UnityEngine.InputSystem;

namespace Core.Player
{
    using UnityEngine;

    public class AimPlayerState : PlayerState
    {
        private Vector2 aimDirection;
        private Vector3 mouseDirection;
        private Camera mainCamera;
        
        private void Awake()
        {
            mainCamera = Camera.main;
            
        }
        
        public override void OnStateEnter()
        {
            StopAllCoroutines();
            if (Gamepad.current != null) 
                StartCoroutine(AimWithJoystick());

            if (DeviceLog.Instance.currentDevice == Mouse.current)
                StartCoroutine(AimWithMouse());
        }
        
        public override void Behave(Vector2 aim)
        {
            aimDirection = aim;
      
        }

        private IEnumerator AimWithMouse()
        {
            while (aimDirection.magnitude > 0.1f)
            {
                Ray ray = mainCamera.ScreenPointToRay(aimDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    Debug.DrawRay(ray.origin, ray.direction * 100, Color.red);
                    if (hit.collider != null)
                    {
                        mouseDirection = ( playerStateInterpretor.transform.position - hit.point ).normalized;
                        Debug.Log(hit.point);
                        playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), aimDirection, PlayerStat.Instance.turnSpeed);

                    }
                }

                yield return null;
            }
        }
        
        private IEnumerator AimWithJoystick()
        {
            while (aimDirection.magnitude > 0.1f)
            {
                playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), aimDirection, PlayerStat.Instance.turnSpeed);
                yield return null;
            }
        }

        private void TurnPlayerTowardAim()
        {
            
        }

        public override void OnStateExit()
        {
            
        }
    }

}
