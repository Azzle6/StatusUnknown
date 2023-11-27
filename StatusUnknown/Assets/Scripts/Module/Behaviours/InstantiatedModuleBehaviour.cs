namespace Module.Behaviours
{
    using Definitions;
    using UnityEngine;

    public class InstantiatedModuleBehaviour : MonoBehaviour
    {
        private CompiledModule compiledModule;
        private ModuleDefinitionSO moduleDefinition;
        private InstantiatedModuleInfo instantiationInfo;

        public void Init(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            this.compiledModule = compiledModule;
            this.moduleDefinition = this.compiledModule.module.definition;
            this.instantiationInfo = info;
            this.BuildModule();
        }

        private void BuildModule()
        {
            if (this.moduleDefinition.ModuleType != E_ModuleType.BEHAVIOUR)
            {
                Debug.LogWarning($"Trying to instantiate {this.moduleDefinition.ModuleType} which is not a behaviour module. Self destroy.");
                Destroy(this.gameObject);
            }
            
            BehaviourModuleDefinitionSO behaviourDefinition = this.moduleDefinition as BehaviourModuleDefinitionSO;
            
            switch (behaviourDefinition.Behaviour)
            {
                case ProjectileBehaviourData data:
                    this.ApplyProjectileBehaviour(data);
                    break;
                case ZoneBehaviourData data:
                    this.ApplyZoneBehaviour(data);
                    break;
                case DropBehaviourData data:
                    this.ApplyDropBehaviour(data);
                    break;
            }

            foreach (var trigger in this.compiledModule.triggersNextModule)
            {
                if(trigger.compiledModule != null)
                    InstantiateModuleBehaviour(trigger.compiledModule, new InstantiatedModuleInfo(transform.position, transform.forward));
            }
        }

        private void ApplyProjectileBehaviour(ProjectileBehaviourData data)
        { 
            Debug.Log($"Projectile behaviour. shape :{data.Shape}, speed {data.speed}.");
        }

        private void ApplyZoneBehaviour(ZoneBehaviourData data)
        {
            Debug.Log($"Zone behaviour. shape {data.Shape}.");
        }

        private void ApplyDropBehaviour(DropBehaviourData data)
        {
            Debug.Log($"Drop behaviour. Object to drop : {data.instantiatedObject}.");
        }

        public static void InstantiateModuleBehaviour(CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            GameObject obj = new GameObject("module", typeof(InstantiatedModuleBehaviour));
            obj.GetComponent<InstantiatedModuleBehaviour>().Init(compiledModule, info);
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
