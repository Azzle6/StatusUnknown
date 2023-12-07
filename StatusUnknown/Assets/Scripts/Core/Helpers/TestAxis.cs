using UnityEngine;

public class TestAxis : MonoBehaviour
{
    public float angle;

    private void OnDrawGizmos()
    {
        Quaternion curRotation = this.transform.rotation;
        Vector3 upVector = curRotation * Vector3.up;
        Quaternion newRotation = Quaternion.AngleAxis(this.angle, upVector);
        Vector3 result = newRotation * transform.forward;
        
        Gizmos.DrawRay(transform.position, upVector * 0.3f);
        Gizmos.DrawRay(transform.position, result * 0.3f);
    }
}
