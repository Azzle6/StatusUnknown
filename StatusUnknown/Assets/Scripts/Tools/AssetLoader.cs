using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.ResourceManagement.ResourceLocations;
using System.Collections.Generic;
using System;
using System.Reflection;

public enum AssetLoadingType { FromAddress, FromReference, Scene, ByLabel, MemoryOptimised }

// lorsqu'il faudra build le jeu et mieux comprendre le workflow de ce plugin, cf
// pour les bases : https://learn.unity.com/tutorial/build-content-using-addressables?uv=2022.1&courseId=64255c01edbc2a268fb0b800#645dc516edbc2a5a5e61ab22
// pour le profiling : https://learn.unity.com/tutorial/manage-builds-with-profiles?uv=2022.3&courseId=64255c01edbc2a268fb0b800#645dc642edbc2a5a33028010 

// pour gérer une grosse quantité de contenu (déjà créé deux custom groups -> Assets/AddressableAssetsData/AssetGroups) :
// partie 4-7 : https://learn.unity.com/tutorial/organize-addressable-assets-into-groups 
// topics plus avancés -> partie 1, 2 et 3 https://learn.unity.com/tutorial/organize-addressable-assets-into-groups 
// et aussi https://docs.unity3d.com/Manual/AssetBundlesIntro.html

// https://learn.unity.com/tutorial/label-addressable-assets
// super utile pour gestion memoire (utilisation avancée de Loading by Label
// peut aussi servir dans le Bundle Mode -> Pack Together by label (cf partie 6)


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

    // loading by label
    private List<string> _WeaponKeys = new List<string>() { "enemy", "combat", "basic" };
    private AsyncOperationHandle<IList<GameObject>> _LoadByLabelOpHandle;

    // loading by label, better memory management (resource locators)
    private List<string> _EnemyKeys = new List<string>() { "Hats", "Fancy" };
    private AsyncOperationHandle<IList<IResourceLocation>> m_HatsLocationsOpHandle; // _LoadByResourceLocationOpHandle
    private AsyncOperationHandle<GameObject> m_HatLoadOpHandle; // ResourceLocationOpHandle

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
        SetHandle_SpriteReference(); 
    }

    void Start()
    {
        if (!loadAllOnStart) return;

        switch(_LoadingType)
        {
            case AssetLoadingType.FromAddress:
                SetHandle_ByAdress(_Adress);
                break; 
            case AssetLoadingType.FromReference:
                SetHandle_ByAssetReference(_AssetReference); 
                break;
            case AssetLoadingType.Scene:
                SetHandle_Scene();
                break;
            case AssetLoadingType.ByLabel:
                SetHandles_ByLabel();
                break;
            case AssetLoadingType.MemoryOptimised:
                SetHandles_ByLabel_MemoryOptimised();
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

    #region Setters
    /// <summary>
    /// Easiest way to load an asset, but there are no safeguards against typos like for AssetReference
    /// </summary>
    private void SetHandle_ByAdress(string adress)
    {
        if (string.IsNullOrEmpty(adress)) return;

        _WeaponLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(adress);
        OnObjectLoad_AddListener();
    }

    /// <summary>
    /// The call to RuntimeKeyIsValid checks to find out if the value chosen in the object picker is a valid address. 
    /// In this context, it will forbid LoadAssetAsync from being called if the AssetReferenceGameObject is set to None.
    /// </summary>
    private void SetHandle_ByAssetReference(AssetReferenceGameObject assetReference)
    {
        
        if (!assetReference.RuntimeKeyIsValid()) return;

        _WeaponLoadOpHandle = assetReference.LoadAssetAsync<GameObject>();
        OnObjectLoad_AddListener();
    }

    private void SetHandle_SpriteReference()
    {
        _SpriteLoadOpHandle = Addressables.LoadAssetAsync<Sprite>(_SpriteAddress);
        OnSpriteLoad_AddListener();
    }

    private void SetHandles_ByLabel()
    {
        _LoadByLabelOpHandle = Addressables.LoadAssetsAsync<GameObject>(_EnemyKeys, null, Addressables.MergeMode.Intersection); // to only get enemies that are of basic type
        OnLoadByLabels_AddListener(); 
    }

    private void SetHandles_ByLabel_MemoryOptimised()
    {
        m_HatsLocationsOpHandle = Addressables.LoadResourceLocationsAsync(_EnemyKeys, Addressables.MergeMode.Intersection);
        OnLoadByLabels_MemoryOptimised_AddListener(); 
    }

    public static void SetHandle_Scene()
    {
        _SceneLoadOpHandle = Addressables.LoadSceneAsync("WeaponsTests", loadMode : LoadSceneMode.Additive, activateOnLoad: true) ;

    }
    #endregion

    #region Listeners
    private void OnObjectLoad_AddListener()
    {
        _WeaponLoadOpHandle.Completed += OnGameobjectLoadCompleted;
    }

    private void OnSpriteLoad_AddListener()
    {
        _SpriteLoadOpHandle.Completed += OnSpriteLoadCompleted;
    }

    private void OnLoadByLabels_AddListener()
    {
        _LoadByLabelOpHandle.Completed += OnLoadByLabelsCompleted;
    }

    private void OnLoadByLabels_MemoryOptimised_AddListener()
    {
        m_HatsLocationsOpHandle.Completed += OnLoadByLocationCompleted;
    }
    #endregion

    #region Events
    private void OnGameobjectLoadCompleted(AsyncOperationHandle<GameObject> handle)
    {
        Debug.Log($"Status {handle.Status} : for {handle.Result}");
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            _WeaponInstance = Instantiate(handle.Result, Vector3.zero, Quaternion.identity);
        }
    }

    private void OnSpriteLoadCompleted(AsyncOperationHandle<Sprite> handle)
    {
        Debug.Log($"Status {handle.Status} : for {handle.Result}");
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GetComponent<SpriteRenderer>().sprite = handle.Result;
        }
    }

    private void OnLoadByLabelsCompleted(AsyncOperationHandle<IList<GameObject>> handle)
    {
        Debug.Log("AsyncOperationHandle Status: " + handle.Status);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<GameObject> results = handle.Result;
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log("Instantiated Asset name : " + results[i].name);
                Instantiate(results[i], new Vector3(i*5, 0, 0), Quaternion.identity);
            }
        }
    }
    private void OnLoadByLocationCompleted(AsyncOperationHandle<IList<IResourceLocation>> handle)
    {
        Debug.Log("AsyncOperationHandle Status: " + handle.Status);

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            IList<IResourceLocation> results = handle.Result;
            for (int i = 0; i < results.Count; i++)
            {
                Debug.Log("Instantiated Asset name : " + results[i].PrimaryKey);
            }

            LoadRandomFromLocation(results);
        }
    }

    private void LoadRandomFromLocation(IList<IResourceLocation> list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        IResourceLocation tempResourceLocation = list[randomIndex];

        m_HatLoadOpHandle = Addressables.LoadAssetAsync<GameObject>(tempResourceLocation);
        m_HatLoadOpHandle.Completed += OnGameobjectLoadCompleted;
    }
    #endregion

    private void OnDisable()
    {
        if (!_WeaponLoadOpHandle.IsValid()) return; 

        _WeaponLoadOpHandle.Completed -= OnGameobjectLoadCompleted;
        _LoadByLabelOpHandle.Completed -= OnLoadByLabelsCompleted; 
        m_HatsLocationsOpHandle.Completed -= OnLoadByLocationCompleted;
    }
}