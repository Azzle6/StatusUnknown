using UnityEngine;
using UnityEngine.UIElements;

// Editor, Window, PropertyDrawer
namespace StatusUnknown.CoreGameplayContent
{
    // [RequireComponent(typeof(UIDocument))]
    public class GameplayManager : MonoBehaviour
    {
        // save combat config
        // save encounter and store enemy positions;

        [Header("Combat")]
        public CombatSimulatorScriptableObject _CombatSimulator;
    }
}
