using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;

public enum AssetLoadingType { FromAddress, FromReference, Scene }

// lorsqu'il faudra build le jeu et mieux comprendre le workflow de ce plugin, cf
// pour les bases : https://learn.unity.com/tutorial/build-content-using-addressables?uv=2022.1&courseId=64255c01edbc2a268fb0b800#645dc516edbc2a5a5e61ab22
// pour le profiling : https://learn.unity.com/tutorial/manage-builds-with-profiles?uv=2022.3&courseId=64255c01edbc2a268fb0b800#645dc642edbc2a5a33028010 

// pour gérer une grosse quantité de contenu (déjà créé deux custom groups -> Assets/AddressableAssetsData/AssetGroups) :
// partie 4-7 : https://learn.unity.com/tutorial/organize-addressable-assets-into-groups 
// topics plus avancés -> partie 1, 2 et 3 https://learn.unity.com/tutorial/organize-addressable-assets-into-groups 
// et aussi https://docs.unity3d.com/Manual/AssetBundlesIntro.html

public class AssetLoader : MonoBehaviour
{
    [SerializeField] private bool loadAllOnStart;

    [Header("Addressables")]
    [SerializeField] private string _Adress;
    [SerializeField] private AssetLoadingType _LoadingType;
    private AsyncOperationHandle<GameObject> _WeaponLoadOpHandle;
    private GameObject _WeaponInstance; 

    [Space, SerializeField] private string _SpriteAddress; // can also work with AssetReferenceSprite
    private AsyncOperationHandle<Sprite> _SpriteLoadOpHandle;

    //[Space, SerializeField] private AssetReference _SceneReference; 
    private static AsyncOperationHandle<SceneInstance> _SceneLoadOpHandle;
    private AsyncOperation _AsyncLoadOperation; 

    /// <summary>
    /// On the surface, you'll add AssetReferences to your scripts just like you would add direct references, through public fields and private serializable fields. 
    /// AssetReferences do not store a direct reference to the asset. The AssetReference stores the global unique identifier (GUID) of the asset, 
    /// which is used by the Addressables system to store the object for retrieval at runtime.
    /// The basic AssetReference allows to load any typ of asset (scene, texture, etc..)
    /// </summary>
    [SerializeField] private AssetReferenceGameObject _AssetReference;

    /// <summary>
    /// Handles can be set in the OnEnable function because they already live in the Editor before any runtime call
    /// </summary>
    private void OnEnable()
    {
        _SpriteLoadOpHandle = Addressables.LoadAssetAsync<Sprite>(_SpriteAddress);
        OnSpriteLoadComplete(); 
    }

    void Start()
    {
        if (!loadAllOnStart) return;

        switch(_LoadingType)
        {
            case AssetLoadingType.FromAddress:
                SetHandle(_Adress);
                break; 
            case AssetLoadingType.FromReference:
                SetHandle(_AssetReference); 
                break;
            case AssetLoadingType.Scene:
                LoadNextLevel();
                break;
        }
    }

    private void Update()
    {
        
        /* if (Input.GetMouseButtonUp(1) && _WeaponInstance)
        {
            Destroy(_WeaponInstance); // unload from memory
            Addressables.ReleaseInstance(_WeaponInstance); 
        } */
    }

    /// <summary>
    /// Easiest way to load an asset, but there are no safeguards against typos like for AssetReference
    /// </summary>
    private void SetHandle(string adress)
    {
        if (string.IsNullOrEmpty(adress)) return;

        _WeaponLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(adress);
        OnObjectLoadComplete();
    }

    /// <summary>
    /// The call to RuntimeKeyIsValid checks to find out if the value chosen in the object picker is a valid address. 
    /// In this context, it will forbid LoadAssetAsync from being called if the AssetReferenceGameObject is set to None.
    /// </summary>
    private void SetHandle(AssetReferenceGameObject assetReference)
    {
        
        if (!assetReference.RuntimeKeyIsValid()) return;

        _WeaponLoadOpHandle = assetReference.LoadAssetAsync<GameObject>();
        OnObjectLoadComplete();
    }

    public static void LoadNextLevel()
    {
        _SceneLoadOpHandle = Addressables.LoadSceneAsync("WeaponsTests", loadMode : LoadSceneMode.Additive, activateOnLoad: true) ;

    }

    private void OnObjectLoadComplete()
    {
        _WeaponLoadOpHandle.Completed += RaiseGameplayEvent;
    }

    private void OnSpriteLoadComplete()
    {
        _SpriteLoadOpHandle.Completed += RaiseVisualEvent;
    }

    private void RaiseGameplayEvent(AsyncOperationHandle<GameObject> handle)
    {
        Debug.Log($"Status {handle.Status} : for {handle.Result}");
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _WeaponInstance = Instantiate(handle.Result, Vector3.zero, Quaternion.identity);  
        }
    }

    private void RaiseVisualEvent(AsyncOperationHandle<Sprite> handle)
    {
        Debug.Log($"Status {handle.Status} : for {handle.Result}");
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GetComponent<SpriteRenderer>().sprite = handle.Result;
        }
    }

    private void OnDisable()
    {
        if (!_WeaponLoadOpHandle.IsValid()) return; 

        _WeaponLoadOpHandle.Completed -= RaiseGameplayEvent;
    }
}
