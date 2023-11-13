using UnityEngine;

namespace StatusUnknown.CoreGameplayContent
{
    public struct AbilityInfos
    {
        public string Name { get; private set; } 
        public EPayloadType PayloadType { get; private set; }
        public GameObject Area { get; private set; }    
        public int PayloadValue { get; private set; }

        public AbilityInfos(string name, EPayloadType plType, GameObject area, int payloadValue)
        {
            Name = name;    
            PayloadType = plType;   
            Area = area;
            PayloadValue = payloadValue;
        }
    }

    public class AbilityConfigSO_Base : ScriptableObject
    {
        [SerializeField] protected EAbilityType abilityType = EAbilityType.Offense;
        [SerializeField, Range(1, 100)] protected int payload = 1;

        [Space, SerializeField] protected GameObject effectArea;

        protected EPayloadType payloadType = EPayloadType.OverTime;

        public virtual (AbilityInfos infos, AbilityConfigSO_Base so) GetAbilityData()
        {
            return (new AbilityInfos(name, payloadType, effectArea, payload), this);
        }
    }
}
