using UnityEngine;
using UnityEngine.Pool; 

// later on -> keybindings or any way to automatically create/delete a scriptable object (module, weapon, enemy)
// prevents having to countlessly navigate throuh sub-menus to create a scriptableObject

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "CombatSimulator_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Simulator", order = 1)]
    public class CombatSimulatorScriptableObject : ScriptableObject
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
