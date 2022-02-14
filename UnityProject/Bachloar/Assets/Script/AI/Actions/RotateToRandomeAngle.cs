using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RotateToRandomeAngle", menuName = "PluggableAI/Actions/Movement/RotateToRandomeAngle")]
public class RotateToRandomeAngle : PluggableAction
{
    public float minAngle;
    public float maxAngle;
    public float rotationSpeed;
    bool rotRight;

    Vector2 toRotPos;

    public override void Act(StateController sceneRef)
    {
        Vector2 rotationDir = toRotPos+ (Vector2)sceneRef.transform.position - (Vector2)sceneRef.transform.position;
        rotationDir = rotationDir.normalized;
        float targetAngel = Mathf.Atan2(rotationDir.y, rotationDir.x) * Mathf.Rad2Deg - 90f;
        Quaternion q = Quaternion.AngleAxis(targetAngel, Vector3.forward);
        sceneRef.transform.rotation = Quaternion.Slerp(sceneRef.transform.rotation, q, rotationSpeed * Time.deltaTime);
        if(Vector2.Angle(sceneRef.transform.up,rotationDir) < 1)
        {
            float randomeAngle = Random.Range(minAngle, maxAngle);
            if (rotRight)
            {
                Quaternion rQ = Quaternion.AngleAxis(randomeAngle, Vector3.forward);
                toRotPos = rQ * sceneRef.transform.up;
                rotRight = false;
            }
            else
            {
                Quaternion rQ = Quaternion.AngleAxis(-randomeAngle, Vector3.forward);
                toRotPos = rQ * sceneRef.transform.up;
                rotRight = true;
            }
        }
    }

    public override void OnActEnd(StateController sceneRef)
    {

    }

    public override void OnActInit(StateController sceneRef)
    {
        
    }

    public override void OnActStart(StateController sceneRef)
    {
        rotRight = Random.Range(0.0f, 1.0f) < 0.5f;
        float randomeAngle = Random.Range(minAngle, maxAngle);
        if (rotRight)
        {
            Quaternion q = Quaternion.AngleAxis(randomeAngle, Vector3.forward);
            toRotPos = q * sceneRef.transform.up;
            rotRight = false;
        }
        else
        {
            Quaternion q = Quaternion.AngleAxis(-randomeAngle, Vector3.forward);
            toRotPos = q * sceneRef.transform.up;
            rotRight = true;
        }
    }
}
