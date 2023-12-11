using System;
using System.Collections.Generic;
using Core.VariablesSO.VariableTypes;
using Player;

namespace Augment
{
    using UnityEngine;

    public class AugmentManager : MonoBehaviour
    {
        public PlayerStateInterpretor PlayerStateInterpretor;
        [SerializeField] private Augment[] currentAugments;

        private void Start()
        {
            InitAugmentManager();
        }

        private void InitAugmentManager()
        {
            for (int x = 0; x < currentAugments.Length; x++)
            {
                if (currentAugments[x] == default)
                    break;
                PlayerInfoUIHandler.Instance.UpdateAugmentIcon(x,currentAugments[x].augmentSprite);
            }
        }

        public void AugmentUse(int AugmentIndex)
        {
            if (currentAugments[AugmentIndex] == default) 
                return;

            if (!currentAugments[AugmentIndex].isReady)
                return;

            currentAugments[AugmentIndex].ActionPressed();
            PlayerInfoUIHandler.Instance.AugmentUsed(AugmentIndex, currentAugments[AugmentIndex].augmentCooldown);
        }
        
    }
}

