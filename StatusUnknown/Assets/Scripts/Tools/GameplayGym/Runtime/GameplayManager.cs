using UnityEngine;

// Editor, Window, PropertyDrawer
namespace StatusUnknown.CoreGameplayContent
{
    public struct Payload { public int value; }

    // [RequireComponent(typeof(UIDocument))]
    public class GameplayManager : MonoBehaviour
    {
        // save combat config
        // save encounter and store enemy positions;

        [Header("Combat")]
        public CombatSimulatorScriptableObject CombatSimulator;
        EDamageType EDamageType;
        private Payload payload = new Payload(); 

        private void Start()
        {
            EDamageType = CombatSimulator.GetRootValue();
            ApplyDamage(EDamageType); 
        }

        private void OnDamageDone()
        {
            EDamageType = CombatSimulator.GetNextValue();
            ApplyDamage(EDamageType);
        }

        private void ApplyDamage(EDamageType dmg)
        {

        }
    }
}
