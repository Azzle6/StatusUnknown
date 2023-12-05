namespace Module
{
    using Definitions;

    public class CompiledModule
    {
        public ModuleData module;
        public CompiledOutputInfo[] triggersNextModule;

        public CompiledModule(ModuleData moduleData)
        {
            this.module = moduleData;
            this.triggersNextModule = null;
        }
    }

    public struct CompiledOutputInfo
    {
        public E_ModuleOutput moduleTrigger;
        public CompiledModule compiledModule;

        public CompiledOutputInfo(E_ModuleOutput moduleTrigger, CompiledModule compModule)
        {
            this.moduleTrigger = moduleTrigger;
            this.compiledModule = compModule;
        }
    }
}
