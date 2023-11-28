namespace Module.Behaviours
{
    using System;
    using System.Collections;
    using Definitions;
    using UnityEngine;

    public class InstantiatedModule : MonoBehaviour
    {
        public CompiledModule CompiledModule;

        protected Action OnSpawnEvent;
        protected Action OnTickEvent;
        protected Action OnHitEvent;

        protected InstantiatedModuleInfo currentInfo;

        private void Start()
        {
            this.OnSpawnEvent?.Invoke();
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
                this.OnTickEvent?.Invoke();
            }
        }

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
            this.currentInfo = info;

            foreach (var trigger in CompiledModule.triggersNextModule)
            {
                if (trigger.compiledModule != null)
                {
                    switch (trigger.weaponTrigger)
                    {
                        case E_ModuleOutput.ON_SPAWN:
                            this.OnSpawnEvent += () => this.TriggerNextModule(trigger.compiledModule);
                            break;
                        case E_ModuleOutput.ON_HIT:
                            this.OnHitEvent += () => this.TriggerNextModule(trigger.compiledModule);
                            break;
                        case E_ModuleOutput.ON_TICK:
                            this.OnTickEvent += () => this.TriggerNextModule(trigger.compiledModule);
                            this.StartCoroutine(Tick(0.3f));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private void TriggerNextModule(CompiledModule nextModule)
        {
            ModuleBehaviourHandler.Instance.GetModuleBehaviourToInstantiate(nextModule, this.currentInfo);
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
