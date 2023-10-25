using UnityEngine;
using Yarn.Unity;

// Can't call command facial_expression, because LeftMarker doesn't have the correct component

public class CustomCommandsBase : MonoBehaviour
{

    // Drag and drop your Dialogue Runner into this variable.
    public DialogueRunner dialogueRunner;

    public void Awake()
    {
        //dialogueRunner.AddCommandHandler<GameObject>("camera_moveTo", CameraMoveTo);
    }

    [YarnCommand("facial_expression")]
    public void SetNPCFacialExpression(GameObject target, bool isHappy = false)
    {
        if (isHappy)
        {
            // set target facial expression to happy
            Debug.Log("the NPC is happy"); 
        }
        else
        {
            // set target facial expression to sad
            Debug.Log("the NPC is sad");
        }
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
