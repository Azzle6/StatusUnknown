/*namespace Core.Player
{
    using System.Collections;
    using System.Collections.Generic;
    using Core.Player;
    using UnityEngine;

    public class ShootingPlayerState : PlayerState
    {
        private Transform snapTo;
        private int closestTargetIndex;
        private Vector3[] midPointToTarget;
        private Vector3[] outPointToTarget;
        private RaycastHit[] snapHitsIn;
        private Ray reverseRay;
        private RaycastHit reverseHit;
        private float bestdistanceToClosestTarget;

        private Ray playerToMouseRay;


        public override void OnStateEnter()
        {
            throw new System.NotImplementedException();
        }
        
        private void SnapToTarget()
        {
            if (snapHitsIn.Length == 0)
                yield return null;
                        
            midPointToTarget = new Vector3[snapHitsIn.Length];
            outPointToTarget = new Vector3[snapHitsIn.Length];
            bestdistancetotarget = 1000;
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
                    
                    //playerStateInterpretor.transform.LookAt(snapTo.transform.parent.position);
                    //snapHitsIn = Physics.RaycastAll(playerToMouseRay, 50);
                    //playerToMouseRay = new Ray(playerStateInterpretor.transform.position, mouseDirection);



                }
            }
                        
                        
            Debug.DrawRay(playerStateInterpretor.transform.position, mouseDirection * 50, Color.green);
                    
        } 
        
        public override void OnStateExit()
        {
            throw new System.NotImplementedException();
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

        
}*/
