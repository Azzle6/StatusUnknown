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
        public E_ModuleOutput weaponTrigger;
        public CompiledModule compiledModule;

        public CompiledOutputInfo(E_ModuleOutput weaponTrigger, CompiledModule compModule)
        {
            this.weaponTrigger = weaponTrigger;
            this.compiledModule = compModule;
        }
    }
}
