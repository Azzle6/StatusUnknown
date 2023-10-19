using System;

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
        private int closestTargetIndex;
        private float bestdistanceToClosestTarget;

        private Vector3[] midPointToTarget;
        private Vector3[] outPointToTarget;
        private RaycastHit[] snapHitsIn;
        private Ray reverseRay;
        private RaycastHit reverseHit;

        private Ray camRay;
        private RaycastHit camHit;
        
        private Ray playerToMouseRay;

        
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
                //cam to mouse pos ray
                camRay = mainCamera.ScreenPointToRay(aimDirection);
                if (Physics.Raycast(camRay, out camHit, 100))
                {
                    if (camHit.collider != default)
                        yield return null;
                    
                    mouseDirection = (camHit.point - playerStateInterpretor.transform.position ).normalized;
                    mouseDirection.y = 0;
                    bestdistanceToClosestTarget = 100;
                    //player to mouse pos ray 
                    playerToMouseRay = new Ray(playerStateInterpretor.transform.position, mouseDirection);
                    snapHitsIn = Physics.RaycastAll(playerToMouseRay, 50);
                        
                    SnapPlayerToTarget();
                    if (snapHitsIn.Length == 0)
                        yield return null;
                        
                    midPointToTarget = new Vector3[snapHitsIn.Length];
                    outPointToTarget = new Vector3[snapHitsIn.Length];

                    for (int x = 0; x < snapHitsIn.Length; x++)
                    {
                        reverseRay = new Ray(snapHitsIn[x].point+ (mouseDirection*30), -mouseDirection);
                        Debug.DrawRay(snapHitsIn[x].point+ (mouseDirection*30), -mouseDirection * 50, Color.yellow);
                        if (snapHitsIn[x].collider.Raycast(reverseRay, out reverseHit, 50f))
                        {
                            midPointToTarget[x] = (snapHitsIn[x].point + reverseHit.point) / 2;
                            outPointToTarget[x] = reverseHit.point;
                            bestdistanceToClosestTarget = Vector3.Distance(snapHitsIn[x].point, reverseHit.point);

                            if (Vector3.Distance(snapHitsIn[x].point, reverseHit.point) < bestdistanceToClosestTarget)
                            {
                                closestTargetIndex = x;
                                snapTo = snapHitsIn[x].collider.transform;
                            }
                        }
                    }
                        
                        
                    Debug.DrawRay(playerStateInterpretor.transform.position, mouseDirection * 50, Color.green);
                    
                }

                yield return null;
            }
        }

        private void SnapPlayerToTarget()
        {
            if (snapTo != null)
            {
                playerStateInterpretor.transform.LookAt(snapTo.transform.parent.position);
            }
            else
            {
                playerStateInterpretor.transform.forward = Vector3.Slerp(new Vector3(playerStateInterpretor.transform.forward.x,0,playerStateInterpretor.transform.forward.z), mouseDirection, PlayerStat.Instance.turnSpeed);
            }
        }
        

        public override void OnStateExit()
        {
            
        }

        private void OnDrawGizmos()
        {
            if (snapHitsIn == null)
                return;
            if (snapHitsIn.Length == 0)
                return;

            for (int x = 0; x < snapHitsIn.Length; x++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(snapHitsIn[x].point, 0.5f);
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(midPointToTarget[x], 0.5f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawSphere(outPointToTarget[x], 0.5f);
            }
        }

       
    }
    

}
