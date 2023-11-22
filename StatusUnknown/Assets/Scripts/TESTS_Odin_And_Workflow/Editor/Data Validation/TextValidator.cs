using Sirenix.OdinInspector.Editor.Validation;

//[assembly: RegisterValidator(typeof(TextValidator))]
public class TextValidator : RootObjectValidator<UnityEngine.UI.Text>
{
    protected override void Validate(ValidationResult result)
    {
        if (string.IsNullOrEmpty(Object.text))
        {
            result.AddWarning("Text field is empty");
        }
        else if (string.Equals(Object.text, "New Text"))
        {
            result.AddWarning("The text field has not been set.");
        }
    }
}

