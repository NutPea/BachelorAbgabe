using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementTargetMovement", menuName = "PluggableAI/Actions/Movement/MovementTargetMovement")]
public class MovementTargetMovement_Action : PluggableAction
{
    MovementTargetHolder movementTargetHolder;
    A_Star_Movement movement;

    public override void Act(StateController sceneRef)
    {
        movement.SetTarget(movementTargetHolder.target.position);
    }

    public override void OnActEnd(StateController sceneRef)
    {
        movement.StopMovement();
    }

    public override void OnActInit(StateController sceneRef)
    {
        movement = sceneRef.GetComponent<A_Star_Movement>();
        movementTargetHolder = sceneRef.GetComponent<MovementTargetHolder>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        movement.StartMovement();
    }
}
