using UnityEngine;
using Yarn.Unity;

public class CustomCommandsBase : MonoBehaviour
{

    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;

    public void Awake()
    {
        dialogueRunner.AddCommandHandler<GameObject>("camera_moveTo", CameraMoveTo);
    }

    private void CameraMoveTo(GameObject target)
    {
        if (target == null)
        {
            Debug.Log("Can't find the target!");
        }

        Camera.main.transform.SetPositionAndRotation(target.transform.position - new Vector3(0,0,20), Quaternion.identity);
    }

}
