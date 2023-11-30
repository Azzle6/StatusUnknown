namespace Module.Behaviours
{
    using System;
    using System.Collections;
    using Definitions;
    using UnityEngine;
    using UnityEngine.Serialization;

    public class InstantiatedModule : MonoBehaviour
    {
        public CompiledModule CompiledModule;

        protected Action<InstantiatedModuleInfo> OnSpawnEvent;
        protected Action<InstantiatedModuleInfo> OnTickEvent;
        protected Action<InstantiatedModuleInfo> OnHitEvent;

        private void Start()
        {
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

        private IEnumerator Tick(float stepDuration)
        {
            while (true)
            {
                yield return new WaitForSeconds(stepDuration);
                this.OnTick();
                this.OnTickEvent?.Invoke(new InstantiatedModuleInfo(transform.position, transform.rotation));
            }
        }

        protected virtual void OnTick()
        { }

        protected virtual void OnUpdate()
        { }

        protected virtual void OnFixedUpdate()
        { }

        protected void BaseInit(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            this.CompiledModule = compiledModule;
            this.transform.position = info.TriggeredPosition;
            this.transform.rotation = info.Rotation;
            

            foreach (var trigger in CompiledModule.triggersNextModule)
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
                            this.StartCoroutine(Tick(0.3f));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private void TriggerNextModule(CompiledModule nextModule, InstantiatedModuleInfo info)
        {
            ModuleBehaviourHandler.Instance.InstantiateModuleBehaviour(nextModule, info);
        }
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
}
