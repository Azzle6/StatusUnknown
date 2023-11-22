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
        [SerializeField] protected EAbilityType abilityType = EAbilityType.Offense; // editor only
        [SerializeField, Range(0, 100)] protected int payloadValue; 

        [Space, SerializeField] protected GameObject effectArea;
        public EAbilityType AbilityType { get; set; }
        public EPayloadType PayloadType { get; protected set; }

        protected AbilityInfos AbilityInfos { get; set; }

        public AbilityConfigSO_Base() { }

        public AbilityConfigSO_Base(AbilityInfos ObjectInitializer)
        {
            AbilityInfos = ObjectInitializer;
        }

        public virtual (AbilityInfos infos, AbilityConfigSO_Base so) GetAbilityData()
        {
            SetAbilityInfos(name, PayloadType, effectArea, payloadValue); 
            return (AbilityInfos, this);
        }

        public virtual void SetAbilityInfos(string name, EPayloadType plType, GameObject area, int payloadValue)
        {
            AbilityInfos = new AbilityInfos(name, plType, area, payloadValue);
            SetData(); 
        }

        protected virtual void SetData()
        {
            payloadValue = AbilityInfos.PayloadValue;
            effectArea = AbilityInfos.Area; 
        }

        public virtual AbilityInfos GetAbilityInfos() => AbilityInfos;
        public virtual GameObject GetArea() => effectArea;
        public virtual void SetArea(GameObject area) {effectArea = area; } 
    }
}
