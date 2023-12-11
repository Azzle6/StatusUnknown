namespace Weapons
{
    using Core.SingletonsSO;
    using Inventory;
    using Inventory.Containers;
    using Inventory.Grid;
    using Inventory.Item;
    using Module;
    using UnityEngine;
    using UnityEngine.UIElements;

    public class TriggerGridView : GridView
    {
        private WeaponTriggerData weaponTriggerData;
        
        public TriggerGridView(VisualElement root, Shape shape, WeaponTriggerData container, E_ItemType[] typesContained) : base(root, shape, container, typesContained)
        {
            this.weaponTriggerData = container;
            this.weaponTriggerData.compiledModules.onNewCompilation += this.OnNewCompilation;
            this.weaponTriggerData.compiledModules.CompileWeaponModules(this.weaponTriggerData.triggerRowPosition, this.weaponTriggerData.modules);
            
            VisualElement outputElement = UIHandler.Instance.uiSettings.triggerTemplate.Instantiate();
            outputElement.Q<VisualElement>("triggerIcon").style.backgroundImage = UIHandler.Instance.outputReferences
                .weaponOutputReferences[this.weaponTriggerData.weaponTriggerType].icon.texture;
            
            outputElement.style.position = Position.Absolute;
            this.gridRoot.Add(outputElement);
            float triggerSize = UIHandler.Instance.uiSettings.triggerSize;
            float slotSize = UIHandler.Instance.uiSettings.slotSize;
            outputElement.transform.position = new Vector3(-triggerSize * 0.75f, slotSize * this.weaponTriggerData.triggerRowPosition + slotSize / 2 - triggerSize/2);
        }

        private void OnNewCompilation(ModuleCompilation newCompilation)
        {
            foreach (var itemView in this.ItemsView)
            {
                bool isCompiledModule = false;
                foreach (var compiledModule in newCompilation.AllCompiledModules)
                {
                    if (itemView.ItemData == compiledModule.module)
                    {
                        foreach (var trigger in compiledModule.triggersNextModule)
                            ((ModuleItemView)itemView).SetLinkView(trigger.moduleTrigger, trigger.compiledModule != null);
                        isCompiledModule = true;
                        break;
                    }
                }

                if (!isCompiledModule)
                {
                    ModuleItemView moduleView = (ModuleItemView)itemView;
                    
                    foreach (var output in moduleView.ModuleItemData.definition.outputs)
                        moduleView.SetLinkView(output.moduleTriggerType, false);
                }
            }
        }

        protected override void OnNewContainerLoad(IItemsDataContainer newContainer)
        {
            this.weaponTriggerData.compiledModules.onNewCompilation -= this.OnNewCompilation;
            this.weaponTriggerData = (WeaponTriggerData)newContainer;
            this.weaponTriggerData.compiledModules.onNewCompilation += this.OnNewCompilation;
            this.weaponTriggerData.compiledModules.CompileWeaponModules(this.weaponTriggerData.triggerRowPosition, this.weaponTriggerData.modules);
        }
        
        
    }
}
