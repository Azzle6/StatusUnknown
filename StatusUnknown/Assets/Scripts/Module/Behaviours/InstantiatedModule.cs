namespace Module.Behaviours
{
    using System;
    using System.Collections;
    using Definitions;
    using UnityEngine;

    public class InstantiatedModule : MonoBehaviour
    {
        public CompiledModule CompiledModule;

        protected Action<InstantiatedModuleInfo> OnSpawnEvent;
        protected Action<InstantiatedModuleInfo> OnTickEvent;
        protected Action<InstantiatedModuleInfo> OnHitEvent;

        private void Start()
        {
            this.OnSpawnEvent?.Invoke(new InstantiatedModuleInfo(transform.position, transform.forward));
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
                this.OnTickEvent?.Invoke(new InstantiatedModuleInfo(transform.position, transform.forward));
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        protected virtual void OnTick()
        {
            Debug.Log("Basic module tick.");
        }

        protected virtual void OnUpdate()
        {
            Debug.Log("Basic module update.");
        }

        protected virtual void OnFixedUpdate()
        {
            Debug.Log("Basic module fixes update.");
        }

        protected void BaseInit(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            this.CompiledModule = compiledModule;

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
            ModuleBehaviourHandler.Instance.GetModuleBehaviourToInstantiate(nextModule, info);
        }
    }
    
    public struct InstantiatedModuleInfo
    {
        public Vector3 TriggeredPosition;
        public Vector3 Direction;

        public InstantiatedModuleInfo(Vector3 triggeredPosition, Vector3 direction)
        {
            this.TriggeredPosition = triggeredPosition;
            this.Direction = direction;
        }
    }
}
