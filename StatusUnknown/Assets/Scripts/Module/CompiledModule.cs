namespace Module
{
    public class CompiledModule
    {
        public ModuleData module;
        public CompiledTriggerInfo[] triggersNextModule;

        public CompiledModule(ModuleData moduleData)
        {
            this.module = moduleData;
            this.triggersNextModule = null;
        }
    }

    public struct CompiledTriggerInfo
    {
        public TriggerSO trigger;
        public CompiledModule compiledModule;

        public CompiledTriggerInfo(TriggerSO trigger, CompiledModule compModule)
        {
            this.trigger = trigger;
            this.compiledModule = compModule;
        }
    }
}
