using Augment;
using UnityEngine;

namespace Core.EventsSO
{
    [CreateAssetMenu(menuName = "CustomAssets/BaseGameEventWithParameter/AugmentDataGameEvent", fileName = "AugmentDataGameEvent", order = 0)]
    public class AugmentDataGameEvent : GameEventWithParameter<AugmentStat>
    {
        
    }

}
