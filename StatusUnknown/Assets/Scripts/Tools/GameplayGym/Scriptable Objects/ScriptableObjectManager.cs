using UnityEngine;
using UnityEngine.Pool; 

// later on -> keybindings or any way to automatically create/delete a scriptable object (module, weapon, enemy)
// prevents having to countlessly navigate through sub-menus to create a scriptableObject

namespace StatusUnknown.CoreGameplayContent
{
    /// <summary>
    /// Base class for better content creation, management, and browsing. 
    /// Can be upgraded with keybinds, searchbar etc.. for our gameplay-related scriptable objects
    /// </summary>
    [CreateAssetMenu(fileName = "ScriptableObjectManager", menuName = "Status Unknown/Scriptable Object Manager", order = 1)]
    public class ScriptableObjectManager : ScriptableObject
    {
        public EScriptableType scriptableObjectType;
        // public GameObject Model;

        public AbilityConfigScriptableObject AbilityConfig;
        // public AudioConfigScriptableObject AudioConfig;
        // public VisualConfigScriptableObject VisualConfig;

        private MonoBehaviour ActiveMonoBehaviour;
        private AudioSource CombatAudioSource;
        private GameObject Model;
        private Camera ActiveCamera;

        private ParticleSystem DefaultParticleSystem;
        private ObjectPool<TrailRenderer> TrailPool;
    }
}
