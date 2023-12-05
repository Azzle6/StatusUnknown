namespace Module.Behaviours
{
    using System;
    using System.Collections;
    using Definitions;
    using UnityEngine;

    [Serializable]
    public abstract class InstantiatedModule : MonoBehaviour, ITest
    {
        private CompiledModule compiledModule;
        private IBehaviourData behaviourData;

        protected Action<InstantiatedModuleInfo> OnSpawnEvent;
        protected Action<InstantiatedModuleInfo> OnTickEvent;
        protected Action<InstantiatedModuleInfo> OnHitEvent;
        
        public void Init(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data)
        {
            this.behaviourData = data;
            this.compiledModule = compiledModule;
            this.transform.position = info.TriggeredPosition;
            this.transform.rotation = info.Rotation;

            foreach (var trigger in this.compiledModule.triggersNextModule)
            {
                if (trigger.compiledModule != null)
                {
                    switch (trigger.weaponTrigger)
                    {
                        case E_ModuleOutput.ON_SPAWN:
                            this.OnSpawnEvent += (i) => this.TriggerNextModule(trigger.compiledModule, i);
                            break;
                        case E_ModuleOutput.ON_HIT:
                            this.OnHitEvent += (i) => this.TriggerNextModule(trigger.compiledModule, i);
                            break;
                        case E_ModuleOutput.ON_TICK:
                            this.OnTickEvent += (i) => this.TriggerNextModule(trigger.compiledModule, i);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            
            if(this.behaviourData.TickRate >= 0)
                this.StartCoroutine(Tick(this.behaviourData.TickRate));

            StartCoroutine(LifeTime(data.LifeTime));
            
            OnInit(compiledModule, info, data);
        }

        protected abstract void OnInit(CompiledModule compiledModule, InstantiatedModuleInfo info, IBehaviourData data);

        private IEnumerator Tick(float stepDuration)
        {
            while (true)
            {
                yield return new WaitForSeconds(stepDuration);
                this.OnTick();
                this.OnTickEvent?.Invoke(new InstantiatedModuleInfo(transform.position, transform.rotation));
            }
        }

        #region OVERRIDABLE_METHODS
        protected virtual void OnStart()
        {}
        
        protected virtual void OnTick()
        { }

        protected virtual void OnUpdate()
        { }

        protected virtual void OnFixedUpdate()
        { }
        
        protected abstract void CollisionBehaviour();
        #endregion
        
        #region UTILITIES
        protected Collider[] CheckCollisions()
        {
            Collider[] result = this.behaviourData.CollisionShape.DetectColliders(this.transform.position, this.transform.rotation,
                this.behaviourData.LayerMask);

            return result;
        }
        #endregion

        private IEnumerator LifeTime(float time)
        {
            yield return new WaitForSeconds(time);
            this.DestroyModule();
        }

        protected abstract void DestroyModule();
        
        private void TriggerNextModule(CompiledModule nextModule, InstantiatedModuleInfo info)
        {
            ModuleBehaviourHandler.Instance.InstantiateModuleBehaviour(nextModule, info);
        }
        
        #region UNITY_METHODS
        private void Start()
        {
            this.OnStart();
            this.OnSpawnEvent?.Invoke(new InstantiatedModuleInfo(transform.position, transform.rotation));
        }

        private void Update()
        {
            this.OnUpdate();
        }

        private void FixedUpdate()
        {
            this.OnFixedUpdate();
        }
        #endregion
    }
    
    public struct InstantiatedModuleInfo
    {
        public Vector3 TriggeredPosition;
        public Quaternion Rotation;
        public Collider LastHit;

        public InstantiatedModuleInfo(Vector3 triggeredPosition, Quaternion rotation, Collider lastHit = null)
        {
            this.TriggeredPosition = triggeredPosition;
            this.Rotation = rotation;
            this.LastHit = lastHit;
        }
    }
    
    public interface ITest{}
}
