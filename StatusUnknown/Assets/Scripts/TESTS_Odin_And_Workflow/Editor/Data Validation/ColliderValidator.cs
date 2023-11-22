using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor.Validation;
using Sirenix.Utilities;
using UnityEngine;

// https://odininspector.com/documentation/sirenix.odininspector.editor.validation.validationresult
// https://odininspector.com/documentation/sirenix.odininspector.editor.validation.resultitem
[assembly: RegisterValidationRule(typeof(GameobjectNeedsCollider))]
public class GameobjectNeedsCollider : RootObjectValidator<GameObject>
{
    public Vector3 center;
    public float radius;
    public Direction direction;

    protected override void Validate(ValidationResult result)
    {
        if (Object.GetComponent<CapsuleCollider>() != null)
        {
            // add message and fix 
            result.AddError($"{Object.name} is missing a collider")
                .WithFix("Add Collider", () => AddCollider(), true);
        }
    }

    protected void AddCollider() { }
}