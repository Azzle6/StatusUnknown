using UnityEngine.VFX;

namespace Augment
{
    using UnityEngine;
    using System.Collections;
    using DG.Tweening;

    public class Dash : Augment
    {
        [SerializeField] private DashStat dashStat;
        [SerializeField] private VisualEffect dashVFX;
        private Vector3 tempTargetPos;
        
        private void Awake()
        {
            augmentCooldown = dashStat.augmentCooldown;
        }
        public override void ActionPressed()
        {
            base.ActionPressed();
            dashVFX.enabled = true;
            dashVFX.Play();
            Physics.Raycast(augmentManager.PlayerStateInterpretor.transform.position, augmentManager.PlayerStateInterpretor.transform.forward, out RaycastHit hit, dashStat.dashLength);
            if (hit.collider != default)
            {
                tempTargetPos = hit.point - augmentManager.PlayerStateInterpretor.transform.forward;
                augmentManager.PlayerStateInterpretor.transform.DOMove(tempTargetPos, dashStat.dashDuration).OnComplete((() => dashVFX.enabled = false));
            }
            else
            {
                augmentManager.PlayerStateInterpretor.transform.DOMove
                    (augmentManager.PlayerStateInterpretor.transform.position + augmentManager.PlayerStateInterpretor.transform.forward * dashStat.dashLength, dashStat.dashDuration).OnComplete(() => dashVFX.enabled= false);
            }
            augmentDataGameEvent.RaiseEvent(dashStat);

            StartCoroutine(AugmentCooldownCoroutine());
        }

        public override AugmentStat GetAugmentStat() 
        {
            return dashStat;
        }

        public override IEnumerator AugmentCooldownCoroutine()
        {
            isReady = false;
            yield return new WaitForSeconds(dashStat.augmentCooldown);
            isReady = true;
        }
    }

}

