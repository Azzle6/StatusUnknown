namespace Module
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using UnityEngine;

    [Serializable]
    public class ModuleCompilation
    {
        private VectorIntModuleDictionary modulesPosition;
        public CompiledModule firstModule;

        public void CompileWeaponModules(int startingRow, VectorIntModuleDictionary modPositions)
        {
            this.modulesPosition = modPositions;

            Vector2Int startingPosition = new Vector2Int(startingRow, 0);
            (ModuleData, Vector2Int) firstModuleInfo = CheckModuleAtPosition(startingPosition);
            this.firstModule = new CompiledModule(firstModuleInfo.Item1);
            this.CalculateLinkedModules(firstModuleInfo.Item2, this.firstModule);
        }

        private void CalculateLinkedModules(Vector2Int currentModulePosition, CompiledModule compiledModule)
        {
            List<CompiledTriggerInfo> triggerInfo = new List<CompiledTriggerInfo>();
            foreach (var output in compiledModule.module.definition.outputs)
            {
                Vector2Int positionToCheck = currentModulePosition + output.localPosition +
                                             GridHelper.DirectionToVectorInt(output.direction);
                
                triggerInfo.Add(new CompiledTriggerInfo(output.triggerType, new CompiledModule(CheckModuleAtPosition(positionToCheck).Item1)));
            }

            compiledModule.triggersNextModule = triggerInfo.ToArray();
        }

        private (ModuleData, Vector2Int) CheckModuleAtPosition(Vector2Int pos) //Return the detected module and his reference position
        {
            foreach (var module in modulesPosition)
            {
                foreach (var position in GridHelper.GetPositionsFromShape(module.Value.GridItemDefinition.shape, module.Key))
                {
                    if (position == pos)
                        return (module.Value, module.Key);
                }
            }
            return (null, Vector2Int.zero);
        }
    }
}
