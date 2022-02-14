using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementShotTarget", menuName = "PluggableAI/Actions/Movement/MovementShotTarget")]
public class MovementShotTarget_Action : PluggableAction
{
    PlayerHolder shotingTargetHolder;
    A_Star_Movement movement;
    Rigidbody2D rb2d;

    public float minTimeBetweenStrave;
    public float maxTimeBetweenStrave;
    float currentTimeBetweenStrave;

    bool isStraving;
    bool isRight;

    public float straveTime;
    float currentStraveTime;

    public float straveSpeed;

    public float minStopDistance;
    public float maxStopDistance;
    float stopDistance;

    public override void Act(StateController sceneRef)
    {


        if (isStraving)
        {
            Vector2 directionVector = Vector2.zero;
            if (isRight)
            {
                directionVector = sceneRef.transform.right;
            }
            else
            {
                directionVector = -sceneRef.transform.right;
            }
            Vector2 movementVector = directionVector * straveSpeed * Time.deltaTime;

            rb2d.MovePosition((Vector2)sceneRef.transform.position + (Vector2)movementVector);

            if(currentStraveTime < 0)
            {
                currentStraveTime = straveTime;
                isStraving = false;
                movement.StartMovement();
            }
            else
            {
                currentStraveTime -= Time.deltaTime;
            }

        }
        else
        {
            float distanceToShotingTarget = Vector2.Distance(sceneRef.transform.position, shotingTargetHolder.target.transform.position);
            if (distanceToShotingTarget < stopDistance)
            {
                movement.StopMovement();
            }
            else
            {
                movement.StartMovement();
            }

            if (currentTimeBetweenStrave < 0)
            {
                currentTimeBetweenStrave = Random.Range(minTimeBetweenStrave, maxTimeBetweenStrave);
                isStraving = true;
                movement.StopMovement();
                isRight = Random.Range(0.0f, 1.0f) < 0.5f;
            }
            else
            {
                currentTimeBetweenStrave -= Time.deltaTime;
            }
        }


        movement.SetTarget(shotingTargetHolder.target.position);
    }

    public override void OnActEnd(StateController sceneRef)
    {
        movement.StopMovement();
    }

    public override void OnActInit(StateController sceneRef)
    {
        movement = sceneRef.GetComponent<A_Star_Movement>();
        shotingTargetHolder = sceneRef.GetComponent<PlayerHolder>();
        rb2d = sceneRef.GetComponent<Rigidbody2D>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        currentTimeBetweenStrave = Random.Range(minTimeBetweenStrave, maxTimeBetweenStrave);
        currentStraveTime = straveTime;
        movement.StartMovement();
        stopDistance = Random.Range(minStopDistance, maxStopDistance);
    }
}
