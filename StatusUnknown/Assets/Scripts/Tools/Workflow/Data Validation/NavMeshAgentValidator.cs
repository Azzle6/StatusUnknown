#if UNITY_EDITOR
using Sirenix.OdinInspector.Editor.Validation;
using UnityEngine; 
using UnityEngine.AI;

[assembly: RegisterValidator(typeof(NavMeshAgentValidator))]

public class NavMeshAgentValidator : RootObjectValidator<NavMeshAgent>
{
    protected override void Validate(ValidationResult result)
    {
        Vector3 pos = Object.transform.position;
        float maxDistance = Object.radius; 

        if (!NavMesh.SamplePosition(pos, out NavMeshHit hit, maxDistance, NavMesh.AllAreas))
        {
            result.AddWarning("Nav mesh agent is not on a nav mesh."); 
        }
    }
}
#endif
