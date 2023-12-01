using Map;

namespace Interactable
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class Teleporter : MonoBehaviour, IInteractable
    {
        [SerializeField] private MapEncyclopedia mapEncyclopedia;
        [SerializeField] private Transform teleporterSas;
        [SerializeField] private Transform teleportPoint;
        public void Interact()
        {
            TeleporterUIManager.Instance.Display();
        }

        private void Teleport()
        {
            
        }
    }
}


