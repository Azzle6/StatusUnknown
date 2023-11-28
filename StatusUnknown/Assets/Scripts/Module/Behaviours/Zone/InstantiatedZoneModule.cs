namespace Module.Behaviours.Zone
{
    using Definitions;

    public class InstantiatedZoneModule : InstantiatedModule
    {
        protected ZoneBehaviourData Data;

        public void Init(ZoneBehaviourData data, CompiledModule compiledModule, InstantiatedModuleInfo info)
        {
            BaseInit(compiledModule, info);
            this.Data = data;
        }
    }
}
