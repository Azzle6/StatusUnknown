namespace Module.Behaviours.Drop
{
    using Definitions;
    
    public class InstantiatedDropModule : InstantiatedModule
    {
        protected DropBehaviourData Data;

        public void Init(DropBehaviourData data, CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            BaseInit(compiledModule, info);
            this.Data = data;
        }
    }
}
