namespace Core.EventsSO.GameEventsTypes
{
    using UnityEngine;

    [CreateAssetMenu(menuName = "CustomAssets/BaseGameEventWithParameter/BoolGameEvent", fileName = "BoolGameEvent", order = 0)]
    public class BoolGameEvent : GameEventWithParameter<bool>
    { }
}
