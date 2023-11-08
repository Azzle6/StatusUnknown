using Sirenix.OdinInspector;

namespace StatusUnknown.CoreGameplayContent
{
    public enum EScriptableType
    {
        NONE = -1,
        Ability,
        Enemy,
        Encounter
    }

    public enum EAbilityType
    {
        Offense,
        Defense,
        Support,
        Control
    }

    public enum EDamageType
    {
        Burst, 
        DOT,
        Delayed
    }
}
