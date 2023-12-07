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
            SceneManager.LoadScene("GameUIWithPlayerInfo", LoadSceneMode.Additive);
            //Addressables.LoadSceneAsync(this.UIScene, LoadSceneMode.Additive);
        }
    }
}
