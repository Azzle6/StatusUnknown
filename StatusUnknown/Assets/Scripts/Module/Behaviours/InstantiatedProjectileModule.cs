namespace Module.Behaviours
{
    using System;
    using System.Collections;
    using Combat.Projectile;
    using Definitions;
    using UnityEngine;

    public abstract class InstantiatedProjectileModule : Projectile, IInstantiatedModule
    {
        public CompiledModule CompiledModule { get; set; }
        protected InstantiatedModuleInfo currentInfo;
        
        protected Action OnSpawnEvent, OnTickEvent, OnHitEvent;
        
        private void Start()
        {
            this.OnSpawnEvent?.Invoke();
        }

        private IEnumerator Tick(float stepDuration)
        {
            while (true)
            {
                yield return new WaitForSeconds(stepDuration);
                Debug.Log("Projectile tick.");
                this.OnTick();
            }
        }

        protected abstract void OnTick();

        protected InstantiatedProjectileModule(CompiledModule compiledModule, InstantiatedModuleInfo info)
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
                            this.StartCoroutine(this.Tick(0.3f));
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        private void TriggerNextModule(CompiledModule nextModule)
        {
            ModuleBehaviourHandler.InstantiateModuleBehaviour(nextModule, this.currentInfo);
        }
    }
}
