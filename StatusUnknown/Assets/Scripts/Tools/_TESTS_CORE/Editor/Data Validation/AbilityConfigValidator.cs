using Sirenix.OdinInspector.Editor.Validation;
using StatusUnknown.Content;
using UnityEditor;
using UnityEngine;

[assembly:RegisterValidator(typeof(ModuleSOValidator))]
public class ModuleSOValidator : RootObjectValidator<AbilityConfigSO_Base>
{
    protected override void Validate(ValidationResult result)
    {
        if (Object.GetArea() == null)
        {
            result.AddWarning("Your effect area is not filled !").WithFix("Add Default Area", () => FixMissingArea());  
        }
    }

    private void FixMissingArea()
    {
        Object.SetArea(AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/damage_area_small.prefab")); 
    }
}
