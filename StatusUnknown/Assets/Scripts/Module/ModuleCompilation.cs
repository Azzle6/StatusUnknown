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
        public int debugLinkedModulesCount;

        public void CompileWeaponModules(int startingRow, VectorIntModuleDictionary modPositions)
        {
            this.debugLinkedModulesCount = 0;
            this.modulesPosition = modPositions;

            Vector2Int startingPosition = new Vector2Int(0, startingRow);
            (ModuleData, Vector2Int) firstModuleInfo = CheckModuleAtPosition(startingPosition);

            if (firstModuleInfo.Item1 != null)
            {
                this.debugLinkedModulesCount = 1;
                this.firstModule = new CompiledModule(firstModuleInfo.Item1);
                this.CalculateLinkedModules(firstModuleInfo.Item2, this.firstModule);
            }
        }

        private void CalculateLinkedModules(Vector2Int currentModulePosition, CompiledModule compiledModule)
        {
            List<CompiledTriggerInfo> triggerInfo = new List<CompiledTriggerInfo>();
            foreach (var output in compiledModule.module.definition.outputs)
            {
                Vector2Int positionToCheck = currentModulePosition + output.localPosition +
                                             GridHelper.DirectionToVectorInt(output.direction, true);
                
                (ModuleData, Vector2Int) nextModuleResult = CheckModuleAtPosition(positionToCheck);
                
                CompiledModule newCompiledModule =
                    nextModuleResult.Item1 == null ? null : new CompiledModule(nextModuleResult.Item1);
                
                triggerInfo.Add(new CompiledTriggerInfo(output.triggerType, newCompiledModule));

                if (nextModuleResult.Item1 != null)
                {
                    this.debugLinkedModulesCount++;
                    if (this.debugLinkedModulesCount >= 30)
                    {
                        Debug.LogWarning("Too many iterations in the compilation, stopped.");
                        return;
                    }
                    CalculateLinkedModules(nextModuleResult.Item2, newCompiledModule);
                }
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
                    {
                        //Debug.Log($"Found a linked module at {pos}.");
                        return (module.Value, module.Key);
                    }
                }
            }
            //Debug.Log($"Didn't find any linked module at {pos}.");
            return (null, Vector2Int.zero);
        }
    }
}
