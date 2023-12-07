using DG.Tweening;
using Map;
using Player;

namespace Interactable
{
    using System.Threading.Tasks;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    public class Teleporter : MonoBehaviour, IInteractable
    {
        [SerializeField] private MapEncyclopedia mapEncyclopedia;
        [SerializeField] private Transform teleporterSas;
        [SerializeField] private Transform teleportPoint;
        [SerializeField] private float sasFallingTime = 2f;
        [SerializeField] private float sasEndPos;
        [SerializeField] private float sasOpenPos;
        private string currentSceneName;
        private TaskCompletionSource<TeleporterData> tcs = new TaskCompletionSource<TeleporterData>();
        
        private void Awake()
        {
            currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        }
        
        public void Interact()
        {
            TeleporterUIManager.Instance.Display(this);
            StartCoroutine(LaunchTeleport());
        }
        
        private IEnumerator LaunchTeleport()
        {
            yield return new WaitUntil(() => tcs.Task.IsCompleted);
            TeleporterData data = tcs.Task.Result;
            tcs = null;
            tcs = new TaskCompletionSource<TeleporterData>();
            teleporterSas.gameObject.SetActive(true);
            teleporterSas.transform.localPosition = new Vector3(0, sasOpenPos, 0);
            teleporterSas.transform.DOLocalMoveY(sasEndPos, sasFallingTime).SetEase(Ease.InOutQuad).OnComplete(() =>
            {
                teleporterSas.gameObject.SetActive(false);
                if(currentSceneName != data.sceneName)
                    UnityEngine.SceneManagement.SceneManager.LoadScene(data.sceneName);
                else
                {
                    PlayerAction.Instance.transform.position = data.teleporterPos;
                    PlayerAction.Instance.EnableEvent();
                    PlayerInfoUIHandler.Instance.Show();

                }
            });
            
        }
        public void ReceiveTeleporterData(TeleporterData data)
        {
            Debug.Log("ReceiveTeleporterData");
            tcs.SetResult(data);
        }

        private void Teleport()
        {
            
        }
    }
}


