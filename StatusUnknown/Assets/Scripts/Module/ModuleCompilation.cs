namespace Module
{
    using System;
    using System.Collections.Generic;
    using Core.Helpers;
    using UnityEngine;

    [Serializable]
    public class ModuleCompilation
    {
        public Action<ModuleCompilation> onNewCompilation;
        public CompiledModule FirstModule;
        public List<CompiledModule> AllCompiledModules = new List<CompiledModule>();
        
        private VectorIntModuleDictionary modulesPosition;

        public void CompileWeaponModules(int startingRow, VectorIntModuleDictionary modPositions)
        {
            this.AllCompiledModules.Clear();
            this.modulesPosition = modPositions;

            Vector2Int startingPosition = new Vector2Int(0, startingRow);
            (ModuleData, Vector2Int) firstModuleInfo = CheckModuleAtPosition(startingPosition);

            if (firstModuleInfo.Item1 != null)
            {
                this.FirstModule = new CompiledModule(firstModuleInfo.Item1);
                this.AllCompiledModules.Add(this.FirstModule);
                this.CalculateLinkedModules(firstModuleInfo.Item2, this.FirstModule);
            }
            this.onNewCompilation?.Invoke(this);
        }

        private void CalculateLinkedModules(Vector2Int currentModulePosition, CompiledModule compiledModule)
        {
            List<CompiledOutputInfo> triggerInfo = new List<CompiledOutputInfo>();
            foreach (var output in compiledModule.module.definition.outputs)
            {
                Vector2Int positionToCheck = currentModulePosition + output.localPosition +
                                             GridHelper.DirectionToVectorInt(output.direction, true);
                
                (ModuleData, Vector2Int) nextModuleResult = CheckModuleAtPosition(positionToCheck);
                
                CompiledModule newCompiledModule =
                    nextModuleResult.Item1 == null ? null : new CompiledModule(nextModuleResult.Item1);
                
                triggerInfo.Add(new CompiledOutputInfo(output.weaponTriggerType, newCompiledModule));

                if (nextModuleResult.Item1 != null)
                {
                    this.AllCompiledModules.Add(newCompiledModule);
                    if (this.AllCompiledModules.Count >= 30)
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
