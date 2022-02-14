using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LookAtMovementTarget", menuName = "PluggableAI/Actions/Movement/LookAtMovementTarget")]
public class LookAtMovementTarget : PluggableAction
{
    Transform sceneRefTransform;
    MovementTargetHolder movementTargetHolder;
    public float rotationSpeed;
    public override void Act(StateController sceneRef)
    {

        Vector2 shotDir = movementTargetHolder.target.transform.position - sceneRefTransform.transform.position;
        shotDir = shotDir.normalized;
        float targetAngel = Mathf.Atan2(shotDir.y, shotDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(targetAngel, Vector3.forward);
        sceneRef.transform.rotation = Quaternion.Slerp(sceneRef.transform.rotation, q, rotationSpeed * Time.deltaTime);
    }

    public override void OnActEnd(StateController sceneRef)
    {

    }

    public override void OnActInit(StateController sceneRef)
    {
        sceneRefTransform = sceneRef.transform;
        movementTargetHolder = sceneRef.GetComponent<MovementTargetHolder>();
    }

    public override void OnActStart(StateController sceneRef)
    {

    }
}
