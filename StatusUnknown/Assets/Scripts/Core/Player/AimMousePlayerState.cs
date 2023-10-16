namespace Core.Player
{
    using UnityEngine;
    using System.Collections;
    
    public class AimMousePlayerState : PlayerState
    {
        private Vector2 aimDirection;
        private Vector3 mouseDirection;
        private Vector3 targetsForward;
        private Camera mainCamera;
        private Transform snapTo;
        
        private void Awake()
        {
            mainCamera = Camera.main;
        }
        
        public override void OnStateEnter()
        {
            StopAllCoroutines();
            StartCoroutine(Aim());
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
                Ray ray = mainCamera.ScreenPointToRay(aimDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider != null)
                    {
                        mouseDirection = (hit.point - playerStateInterpretor.transform.position ).normalized;
                        mouseDirection.y = 0;
                        Ray ray2 = new Ray(playerStateInterpretor.transform.position, mouseDirection);
                        RaycastHit hit2;
                        if (Physics.Raycast(ray2, out hit2, 50))
                        {
                            if (hit2.collider.transform.TryGetComponent(out Target target))
                            {
                                snapTo = target.transform;                 
                            }
                            else
                            {
                                snapTo = default;
                            }
                        }
                        Debug.DrawRay(playerStateInterpretor.transform.position, mouseDirection * 50, Color.green);
                        if (snapTo == default)
                        {
                            playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), mouseDirection, PlayerStat.Instance.turnSpeed);
                        }
                        else
                        {
                           playerStateInterpretor.transform.LookAt(snapTo.transform.parent.position);
                        }
                    }
                }

                yield return null;
            }
        }
        

        public override void OnStateExit()
        {
            
        }
    }

}
