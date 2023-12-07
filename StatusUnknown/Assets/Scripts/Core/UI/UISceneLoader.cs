namespace Core.UI
{
    using UnityEngine;
    using UnityEngine.AddressableAssets;
    using UnityEngine.SceneManagement;

    public class UISceneLoader : MonoBehaviour
    {
        [SerializeField] private AssetReference UIScene;

        private void Awake()
        {
            Addressables.LoadSceneAsync(this.UIScene, LoadSceneMode.Additive);
        }
    }
}
