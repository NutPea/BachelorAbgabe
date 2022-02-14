using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LookAtMovementTargetUpDirection", menuName = "PluggableAI/Actions/Movement/LookAtMovementTargetUpDirection")]
public class LookAtMovementTargetUpDirection_Action : PluggableAction
{
    MovementTargetHolder movementTargetHolder;
    public float rotationSpeed;

    public override void Act(StateController sceneRef)
    {
        Vector2 rotationDir = movementTargetHolder.target.up;
        float targetAngel = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(targetAngel, Vector3.forward);
        sceneRef.transform.rotation = Quaternion.Slerp(sceneRef.transform.rotation, q, rotationSpeed * Time.deltaTime);
    }

    public override void OnActEnd(StateController sceneRef)
    {

    }

    public override void OnActInit(StateController sceneRef)
    {
        movementTargetHolder = sceneRef.GetComponent<MovementTargetHolder>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        
    }
}
