using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Sprint", menuName = "PluggableAI/Actions/Movement/Sprint")]
public class SprintAction : PluggableAction
{
    public float sprintMovementSpeed;
    float savedMovementSpeed;
    A_Star_Movement movement;
    public override void Act(StateController sceneRef)
    {
    }

    public override void OnActEnd(StateController sceneRef)
    {
        movement.movementSpeed = savedMovementSpeed;
    }

    public override void OnActInit(StateController sceneRef)
    {
        movement = sceneRef.GetComponent<A_Star_Movement>();
    }

    public override void OnActStart(StateController sceneRef)
    {
        savedMovementSpeed = movement.movementSpeed;
        movement.movementSpeed = sprintMovementSpeed;
    }
}
