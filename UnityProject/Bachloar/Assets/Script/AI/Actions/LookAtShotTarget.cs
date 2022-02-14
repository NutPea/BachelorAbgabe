using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LookAtShotTarget", menuName = "PluggableAI/Actions/Shoting/LookAtShotTarget")]
public class LookAtShotTarget : PluggableAction
{
    Transform sceneRefTransform;
    PlayerHolder shotingTarget;
    public float rotationSpeed = 20;

    public override void Act(StateController sceneRef)
    { 
        Vector2 shotDir = shotingTarget.target.transform.position - sceneRefTransform.transform.position;
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
        shotingTarget = sceneRef.GetComponent<PlayerHolder>();
    }

    public override void OnActStart(StateController sceneRef)
    {
       
    }
}
