using UnityEngine;
using UnityEngine.Pool; 

namespace StatusUnknown.CoreGameplayContent
{
    [CreateAssetMenu(fileName = "CombatSimulator_Type_Num", menuName = "Status Unknown/Gameplay/Combat/Simulator", order = 1)]
    public class CombatSimulatorScriptableObject : ScriptableObject
    {
        public EAbilityType AbilityType;
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
