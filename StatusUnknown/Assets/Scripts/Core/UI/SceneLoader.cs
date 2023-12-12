namespace Core.UI
{
    using Sirenix.OdinInspector;
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;

    public class SceneLoader : MonoBehaviour
    {
        [SerializeField] private bool loadByAddressable;
        [SerializeField, HideIf("loadByAddressable")] private string sceneName;
        [SerializeField, ShowIf("loadByAddressable"), InfoBox("Scene will be load in async.")] private AssetReference UIScene;

        private void Awake()
        {
            if(this.loadByAddressable)
                Addressables.LoadSceneAsync(this.UIScene, LoadSceneMode.Additive);
            else
                SceneManager.LoadScene(this.sceneName, LoadSceneMode.Additive);
        }
    }
}
