namespace StatusUnknown.Content
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

    public enum EPayloadType
    {
        Burst, 
        OverTime,
        Delayed
    }
}
