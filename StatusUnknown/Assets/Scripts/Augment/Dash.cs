

namespace Augment
{
    using UnityEngine;
    using System.Collections;
    using DG.Tweening;

    public class Dash : Augment
    {
        [SerializeField] private DashStat dashStat;
        private Vector3 tempTargetPos;
        
        private void Awake()
        {
            augmentCooldown = dashStat.augmentCooldown;
        }
        public override void ActionPressed()
        {
            Physics.Raycast(augmentManager.PlayerStateInterpretor.transform.position, augmentManager.PlayerStateInterpretor.transform.forward, out RaycastHit hit, dashStat.dashLength);
            if (hit.collider != default)
            {
                tempTargetPos = hit.point - augmentManager.PlayerStateInterpretor.transform.forward;
                augmentManager.PlayerStateInterpretor.transform.DOMove(tempTargetPos, dashStat.dashDuration);
            }
            else
            {
                augmentManager.PlayerStateInterpretor.transform.DOMove(augmentManager.PlayerStateInterpretor.transform.position + augmentManager.PlayerStateInterpretor.transform.forward * dashStat.dashLength, dashStat.dashDuration);
            }

            StartCoroutine(AugmentCooldownCoroutine());
        }

        public override IEnumerator AugmentCooldownCoroutine()
        {
            isReady = false;
            yield return new WaitForSeconds(dashStat.augmentCooldown);
            isReady = true;
        }
    }

}

