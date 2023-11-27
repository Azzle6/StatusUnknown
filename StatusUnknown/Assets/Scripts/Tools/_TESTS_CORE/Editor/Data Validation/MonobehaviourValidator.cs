using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine;

[assembly: RegisterValidationRule(typeof(MonobehaviourValidator), Name = "Monobehaviour Test Validator")]
public class MonobehaviourValidator : RootObjectValidator<Test_Monobehaviour>
{
    protected override void Validate(ValidationResult result)
    {
        if (Object.GetComponent<Collider>() == null) 
        {
            result.AddError("This script needs a Box Collider."); 
        }
    }
}
